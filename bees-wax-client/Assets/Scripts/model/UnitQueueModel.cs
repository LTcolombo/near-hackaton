using System.Collections.Generic;
using utils.injection;
using utils.signal;
using UnityEngine;

namespace model
{
    [Singleton]
    public class UnitQueueModel
    {
        private const int MaxSize = 7;

        public readonly Signal Updated = new Signal();

        private readonly Queue<GameObject> _data =
            new Queue<GameObject>();


        private readonly List<GameObject> _types =
            new List<GameObject>();

        public void Register(GameObject type)
        {
            if (!_types.Contains(type))
                _types.Add(type);
        }

        public void Push(GameObject value)
        {
            if (_data.Count >= MaxSize)
                return;

            _data.Enqueue(value);

            Updated.Dispatch();
        }

        public Queue<GameObject> Get()
        {
            return _data;
        }

        public GameObject Roll()
        {
            GameObject result;
            if (_data.Count > 0)
            {
                result = _data.Dequeue();
                Updated.Dispatch();
            }
            else
                result = _types[Random.Range(0, _types.Count)];

            return result;
        }
    }
}