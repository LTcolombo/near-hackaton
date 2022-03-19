using System.Collections.Generic;
using utils.injection;
using utils.signal;
using UnityEngine;

namespace model
{
    [Singleton]
    // ReSharper disable once UnusedMember.Global
    public class LayedEggsModel
    {
        private readonly Dictionary<int, GameObject> _data = new Dictionary<int, GameObject>();

        public Signal<int> Updated { get; } = new Signal<int>();

        public GameObject GetLayedEgg(int id)
        {
            return _data.ContainsKey(id) ? _data[id] : null;
        }

        public bool HasLayedEgg(int id)
        {
            return _data.ContainsKey(id);
        }

        public void RemoveLayedEgg(int id)
        {
            if (_data.ContainsKey(id)) _data.Remove(id);
        }

        public void SetLayedEgg(int id, GameObject value)
        {
            if (_data.ContainsKey(id))
            {
                if (value != null && _data[id] == value)
                    return;

                _data.Remove(id);
            }

            _data.Add(id, value);
            Updated.Dispatch(id);
        }
    }
}