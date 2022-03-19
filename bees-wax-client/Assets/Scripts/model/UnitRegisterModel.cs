using System.Collections.Generic;
using model.data;
using utils.injection;
using utils.signal;
using UnityEngine;

namespace model
{
    [Singleton]
    public class UnitRegisterModel
    {
        public readonly Signal<Faction, string> Updated = new Signal<Faction, string>();

        private readonly Dictionary<Faction, Dictionary<string, int>> _data =
            new Dictionary<Faction, Dictionary<string, int>>();

        public void Add(Faction faction, string type)
        {
            if (!_data.ContainsKey(faction))
                _data[faction] = new Dictionary<string, int>();
            
            if (_data[faction].ContainsKey(type))
            {
                _data[faction][type]++;
            }
            else
            {
                _data[faction][type] = 1;
            }

            Updated.Dispatch(faction, type);
        }

        public void Remove(Faction faction, string type)
        {
            if (!_data.ContainsKey(faction))
                _data[faction] = new Dictionary<string, int>();

            if (_data[faction].ContainsKey(type))
            {
                _data[faction][type]--;
                Updated.Dispatch(faction, type);
                if (_data[faction][type] < 0)
                    Debug.Log("negative unit count!");
            }
            else
            {
                Debug.Log("trying to remove a non exisgin unit!");
            }
        }

        public int Get(Faction faction, string type)
        {
            if (!_data.ContainsKey(faction))
                _data[faction] = new Dictionary<string, int>();
            
            return _data[faction].ContainsKey(type) ? _data[faction][type] : 0;
        }
    }
}