using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Nakama;
using Newtonsoft.Json;
using utils.signal;
using UnityEngine;
using utils.injection;

namespace api
{
    [Singleton]
    public class NakamaApi
    {
        public Signal MatchmakerUpdate { get; } = new Signal();
        public Signal ParticipantsUpdated { get; } = new Signal();
        public Signal Error { get; } = new Signal();
        public Signal<long, string, IUserPresence> StateUpdated { get; } = new Signal<long, string, IUserPresence>();

        public List<IUserPresence> Participants;
        public IMatchmakerMatched MatchmakerResult;

        private Client _client;
        private ISession _session;
        private ISocket _socket;

        private IMatchmakerTicket _ticket;
        private IMatch _match;

        private const string StorageKey = "NFT";
        private const string StorageCollection = "storage";

        public void CreateClient()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            var httpRequestAdapter = UnityWebRequestAdapter.Instance;
#else
            var httpRequestAdapter = new HttpRequestAdapter(new HttpClient());
#endif
            _client = new Client("http", "34.89.79.67", 7350, "test-key", httpRequestAdapter);
        }

        public async Task CreateSession(string id = null)
        {
            if (string.IsNullOrEmpty(id))
                id = PlayerPrefs.GetString("nakama.id");
            else
                PlayerPrefs.SetString("nakama.id", id); // cache  id.

            _session = await _client.AuthenticateDeviceAsync(id);
        }

        public async Task CreateSocket()
        {
            _socket = _client.NewSocket();
            await _socket.ConnectAsync(_session);
            _socket.ReceivedMatchmakerMatched += matched =>
            {
                Debug.LogError("ReceivedMatchmakerMatched");
                MatchmakerResult = matched;
                Participants = MatchmakerResult.Users.Select(u => u.Presence).ToList();
                MatchmakerUpdate.Dispatch();
            };

            _socket.ReceivedMatchPresence += OnReceivedMatchPresence;

            var enc = System.Text.Encoding.UTF8;
            _socket.ReceivedMatchState += state =>
            {
                try
                {
                    Debug.Log($"<<<<<{state.OpCode} {state.State}");
                    StateUpdated.Dispatch(state.OpCode, enc.GetString(state.State), state.UserPresence);
                }
                catch (Exception e)
                {
#if UNITY_EDITOR
                    Debug.LogError(e);
#endif
                }
            };

#if UNITY_EDITOR
            _socket.ReceivedError += e =>
            {
                Debug.LogErrorFormat("Socket error: {0}", e.Message);
                Error.Dispatch();
            };
#endif
        }

        private void OnReceivedMatchPresence(IMatchPresenceEvent presence)
        {
            try
            {
                foreach (var elem in presence.Leaves)
                    Participants.Remove(elem);

                Participants.AddRange(presence.Joins);
                ParticipantsUpdated.Dispatch();
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogError(e);
#endif
            }
        }


        public async Task<IEnumerable<IApiUser>> GetUsers(IEnumerable<string> userIds)
        {
            if (_session.HasExpired(DateTime.UtcNow))
                await CreateSession();

            return (await _client.GetUsersAsync(_session, userIds)).Users;
        }

        public async Task GetUser()
        {
            if (_session.HasExpired(DateTime.UtcNow))
                await CreateSession();

            var account = await _client.GetAccountAsync(_session);
            User = account.User;
        }

        public string UserName => User.DisplayName;
        public IApiUser User { get; private set; }

        public async Task SetUserName(string value)
        {
            if (_session.HasExpired(DateTime.UtcNow))
                await CreateSession();

            await _client.UpdateAccountAsync(_session, User.Username, value);

            var account = await _client.GetAccountAsync(_session);
            User = account.User;
        }

        public async void AddMatchmaker(int tier)
        {
            await RemoveMatchmaker();

            _ticket = await _socket.AddMatchmakerAsync(null, 2, 2);
        }

        private async Task RemoveMatchmaker()
        {
            try
            {
                if (_ticket != null)
                    await _socket.RemoveMatchmakerAsync(_ticket);
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogError(e);
#endif
            }

            MatchmakerResult = null;
            MatchmakerUpdate.Dispatch();
        }

        public async Task<IMatch> JoinMatch()
        {
            _match = await _socket.JoinMatchAsync(MatchmakerResult);

            Participants.AddRange(_match.Presences);
            ParticipantsUpdated.Dispatch();

            return _match;
        }

        public async Task SendState(long operation, string value)
        {
            if (_socket != null && _match != null)
                await _socket.SendMatchStateAsync(_match.Id, operation, value);

#if UNITY_EDITOR
            Debug.Log($">>>>{operation} {value}");
#endif
        }

        struct StorageObj
        {
            public string[] tokens;
        }

        public async Task<string[]> GetSelectedNFT()
        {
            var result = await _client.ReadStorageObjectsAsync(_session, new IApiReadStorageObjectId[]
            {
                new StorageObjectId()
                {
                    Collection = StorageCollection,
                    Key = StorageKey,
                    UserId = _session.UserId
                }
            });

            return result.Objects.FirstOrDefault()?.Value == null
                ? null
                : JsonConvert.DeserializeObject<StorageObj>(result.Objects.FirstOrDefault()?.Value).tokens;
        }

        public async Task SaveSelectedNFT(string[] value)
        {
            var result = await _client.WriteStorageObjectsAsync(_session, new IApiWriteStorageObject[]
            {
                new WriteStorageObject
                {
                    Collection = StorageCollection,
                    Key = StorageKey,
                    Value = JsonConvert.SerializeObject(new StorageObj {tokens = value})
                }
            });
        }
    }
}