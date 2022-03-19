using System.Collections.Generic;
using utils.injection;
using utils.signal;

namespace model
{
    [Singleton]
    public class HealthModel
    {
        public readonly Signal<int> Updated = new Signal<int>();

        readonly Dictionary<int, int> _data = new Dictionary<int, int>();

        public int Get(int id)
        {
            return _data.ContainsKey(id)?_data[id] : 0;
        }

        public void Set(int id, int value)
        {
            if (_data.ContainsKey(id) && value <= 0)
                _data.Remove(id);
            else
                _data[id] = value;

            Updated.Dispatch(id);
        }

        public void Apply(int id, int delta)
        {
            if (!_data.ContainsKey(id))
                return;

            var value = _data[id] + delta;
            if (_data.ContainsKey(id) && value <= 0)
                _data.Remove(id);
            else
                _data[id] = value;

            Updated.Dispatch(id);
        }
    }
}