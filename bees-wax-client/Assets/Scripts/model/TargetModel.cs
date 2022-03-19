using System.Collections.Generic;
using utils.injection;
using utils.signal;
using UnityEngine;

namespace model
{
    [Singleton]
    public class TargetModel
    {
        public readonly Signal<int> Updated = new Signal<int>();

        readonly Dictionary<int, GameObject> _data = new Dictionary<int, GameObject>();

        public GameObject GetTarget(int id)
        {
            var containsKey = _data.ContainsKey(id);
            var gameObject = containsKey ? _data[id] : null;
            return gameObject;
        }

        public void SetTarget(int id, GameObject value)
        {
            if (value == null && !_data.ContainsKey(id))
                return;
            
            if (value != null && _data.ContainsKey(id) && _data[id] == value)
                return;

            if (value == null)
                _data.Remove(id);
            else
                _data[id] = value;
            
            Updated.Dispatch(id);
        }
    }
}