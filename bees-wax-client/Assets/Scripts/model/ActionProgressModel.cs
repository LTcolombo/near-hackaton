using System.Collections.Generic;
using model.data;
using utils.injection;
using UnityEngine;

namespace model
{
    public enum ActionType
    {
        Build,
        Hatch,
        Feed
    }

    public class ActionProgress
    {
        public ActionType Type;
        public float Progress;
        public float MaxProgress;
        public Faction Faction;
    }

    [Singleton]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ActionProgressModel
    {
        readonly int[] _maxProgress = {20, 3, 5};

        readonly Dictionary<int, ActionProgress> _data = new Dictionary<int, ActionProgress>();

        public bool Contribute(int id, Faction faction, ActionType type, float value)
        {
            if (!_data.ContainsKey(id) || (_data[id].Type != type && _data[id].Progress >= _data[id].MaxProgress))
            {
                _data[id] = new ActionProgress
                {
                    Type = type,
                    Progress = value,
                    MaxProgress = _maxProgress[(int) type],
                    Faction = faction
                };
            }
            else if (_data[id].Type != type)
            {
#if UNITY_EDITOR
                Debug.LogError("WRONG TYPE: " + _data[id].Type + ":" + type);
#endif
                return false;
            }
            else if (_data[id].Faction != faction)
            {
#if UNITY_EDITOR
                Debug.LogError("WRONG FACTION: " + _data[id].Faction + ":" + faction);
#endif
                return false;
            }

            //ignore contributions after completion
            if (_data[id].Progress >= _data[id].MaxProgress)
                return false;

            _data[id].Progress += value;
            if (_data[id].Progress >= _data[id].MaxProgress)
            {
                _data[id].Progress = _data[id].MaxProgress;
                return false;
            }

            return true;
        }

        public ActionProgress Get(int id)
        {
            return _data.ContainsKey(id) ? _data[id] : null;
        }
    }
}