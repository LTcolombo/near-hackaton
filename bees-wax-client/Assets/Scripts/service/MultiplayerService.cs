using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api;
using model;
using model.data;
using Nakama;
using utils.injection;
using utils.signal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace service
{
    public enum Operation : long
    {
        Ready = 1,
        Create = 2,
        Destroy = 3,
        AssignTarget = 4,
        AssignFaction = 5
    }

    public enum Unit
    {
        None,
        Harvester,
        Nurse,
        Builder,
        Warrior,
        Queen
    }

    [Singleton]
    public class MultiplayerService
    {
        [Inject] private TargetModel _targets;

        [Inject] private FactionModel _faction;

        [Inject] private SelectedTierModel tier;

        [Inject] private NakamaApi _nakama;

        public readonly Signal UsersUpdated = new Signal();
        public readonly Signal Disconnect = new Signal();
        public List<IApiUser> Users => _userMap.Values.OrderByDescending(u => u.Id).ToList();

        private readonly Dictionary<string, IApiUser> _userMap = new Dictionary<string, IApiUser>();
        private readonly Dictionary<int, GameObject> _objectMap = new Dictionary<int, GameObject>();
        private readonly Dictionary<int, int> _reverseLookup = new Dictionary<int, int>();

        private async Task SendData(Operation operation, string data = "")
        {
            if (operation == Operation.Ready)
                AddReadyPlayer(_nakama.User);

            if (_nakama != null)
                await _nakama.SendState((long) operation, data);
        }

        private void OnDataReceived(long op, string data, IUserPresence sender)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() => ProcessData(op, data, sender));
        }

        private void ProcessData(long op, string data, IUserPresence sender)
        {
            if (!Enum.IsDefined(typeof(Operation), op)) return;

            var opEnum = (Operation) op;
            var split = data.Split(':');
            switch (opEnum)
            {
                case Operation.Create:
                    var vectorString = split[2];
                    vectorString = vectorString.Substring(1, vectorString.Length - 2);
                    var vectorCsv = vectorString.Split(',');
                    if (int.TryParse(split[0], out var remoteId) &&
                        float.TryParse(vectorCsv[0], out var x) &&
                        float.TryParse(vectorCsv[1], out var y) &&
                        float.TryParse(vectorCsv[2], out var z))
                    {
                        var prefabName = ((Unit) int.Parse(split[1])).ToString();
                        var position = new Vector3(x, y, z);

                        var hive = GameObject.Find("Hive");
                        var obj = Object.Instantiate(Resources.Load("Prefabs/Units/" + prefabName), position,
                            Quaternion.identity,
                            hive.transform); //todo static access

                        _objectMap[remoteId] = obj as GameObject;
                        _reverseLookup[obj.GetInstanceID()] = remoteId;
                        _faction.Set(obj.GetInstanceID(), Faction.Enemy);
                    }

                    break;
                case Operation.Ready:
                    AddReadyPlayer(_userMap[sender.UserId]);
                    break;
                case Operation.Destroy:
                    if (int.TryParse(data, out remoteId) && _objectMap.ContainsKey(remoteId))
                    {
                        Object.Destroy(_objectMap[remoteId]);
                        _objectMap.Remove(remoteId);
                    }

                    break;
                case Operation.AssignTarget:
                    if (!int.TryParse(split[0], out remoteId))
                        return;

                    if (!int.TryParse(split[1], out var targetId))
                        return;

                    if (_objectMap.TryGetValue(remoteId, out var gameObject))
                    {
                        if (_objectMap.ContainsKey(targetId))
                            _targets.SetTarget(gameObject.GetInstanceID(), _objectMap[targetId]);
                        else
                        {
#if UNITY_EDITOR
                            Debug.LogError($"target {targetId} doesnt exist for {gameObject}");
#endif
                            break;
                        }

                        if (gameObject != null)
                            gameObject.GetComponent<Animator>().SetTrigger("TargetFound");
                    }

                    break;

                case Operation.AssignFaction:
                    if (int.TryParse(data, out remoteId))
                        if (_objectMap.TryGetValue(remoteId, out var commandTarget))
                            _faction.Set(commandTarget.GetInstanceID(), Faction.Enemy);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private const bool AutoJoin = true;

        public int GetPlayerIndex()
        {
            return Users.FindIndex(u => u.Id == _nakama.User.Id);
        }

        private readonly List<string> _readyPlayers = new List<string>();

        private void AddReadyPlayer(IApiUser value)
        {
            if (value == null || _readyPlayers.Contains(value.Id)) return;

            _readyPlayers.Add(value.Id);
            UsersUpdated.Dispatch();
        }

        private async void OnMatchmakerUpdate()
        {
            if (AutoJoin && _nakama.MatchmakerResult != null)
                await _nakama.JoinMatch();
        }

        private void OnParticipantsUpdated()
        {
            UnityMainThreadDispatcher.Instance().Enqueue(ProcessParticipants);
        }

        private async void ProcessParticipants()
        {
            //load missing users
            foreach (var user in await _nakama.GetUsers(_nakama.Participants.Where(p => !_userMap.ContainsKey(p.UserId))
                .Select(p => p.UserId)))
                _userMap[user.Id] = user;

            //trim missing participants
            foreach (var key in _userMap.Keys.ToList())
                if (_nakama.Participants.All(p => p.UserId != key))
                {
                    _userMap.Remove(key);
                    _readyPlayers.Remove(key);
                }

            UsersUpdated.Dispatch();

            if (Users.Count < 2 && _nakama.MatchmakerResult != null)
            {
                if (_readyPlayers.Remove(User?.Id))
                    _nakama.AddMatchmaker(tier.value);
            }
        }

        public bool IsReady(string value)
        {
            return _readyPlayers.Contains(value);
        }

        public Signal UserUpdated { get; } = new Signal();
        public string UserName => User?.DisplayName;
        private IApiUser User => _nakama?.User;

        public async void SetUserName(string value)
        {
            await _nakama.SetUserName(value);
            UserUpdated.Dispatch();

            _readyPlayers.Remove(User?.Id);
            _nakama.AddMatchmaker(tier.value);
        }

        public void InitApi()
        {
            _nakama.MatchmakerUpdate.Add(OnMatchmakerUpdate);
            _nakama.ParticipantsUpdated.Add(OnParticipantsUpdated);
            _nakama.StateUpdated.Add(OnDataReceived);
            _nakama.Error.Add(() => Disconnect.Dispatch());

            _nakama.AddMatchmaker(tier.value);
        }

        public int RegisterObject(GameObject value, int id = -1)
        {
            if (id < 0)
                id = _objectMap.Keys.Count > 0 ? _objectMap.Keys.Max() + 1 : 0;

            _objectMap[id] = value;
            _reverseLookup[value.GetInstanceID()] = id;
            return id;
        }

        public async void AssignTarget(GameObject owner, GameObject target)
        {
            if (_reverseLookup.TryGetValue(owner.GetInstanceID(), out var ownerNetworkId) &&
                _reverseLookup.TryGetValue(target.GetInstanceID(), out var targetNetworkId))
                await SendData(Operation.AssignTarget, $"{ownerNetworkId}:{targetNetworkId}");
#if UNITY_EDITOR
            else
                Debug.LogError("Network ID not found for objects " + owner.name + "," + target.name);
#endif
        }

        public async void Create(GameObject value)
        {
            var unit = Unit.None;
            if (value.name.Contains("Harvester"))
                unit = Unit.Harvester;

            if (value.name.Contains("Nurse"))
                unit = Unit.Nurse;

            if (value.name.Contains("Builder"))
                unit = Unit.Builder;

            if (value.name.Contains("Warrior"))
                unit = Unit.Warrior;

            if (value.name.Contains("Queen"))
                unit = Unit.Queen;

            if (unit != Unit.None)
                await SendData(Operation.Create,
                    RegisterObject(value) + ":" + (int) unit + ":" + value.transform.position);
#if UNITY_EDITOR
            else
                Debug.LogError("Trying to create unregistered prefab type: " + value.name);
#endif
        }

        public async Task Destroy(GameObject value)
        {
            if (_reverseLookup.TryGetValue(value.GetInstanceID(), out var networkId))
                await SendData(Operation.Destroy, networkId.ToString());
#if UNITY_EDITOR
            else
                Debug.LogError("Network ID not found for object " + value.name);
#endif
        }

        public async Task Ready()
        {
            await SendData(Operation.Ready);
        }

        public async void AssignFaction(int instanceId)
        {
            if (_reverseLookup.TryGetValue(instanceId, out var networkId))
                await SendData(Operation.AssignFaction, networkId.ToString());
#if UNITY_EDITOR
            else
                Debug.LogError("Network ID not found for object with instance Id " + instanceId);
#endif
        }
    }
}