using System.Collections.Generic;
using model.data;
using utils.injection;
using utils.signal;

namespace model
{
    [Singleton]
    public class FactionModel
    {
        readonly Dictionary<int, Faction> _data = new Dictionary<int, Faction>();

        public Signal<int> Updated { get; } = new Signal<int>();

        public Faction Get(int id)
        {
            return _data.ContainsKey(id) ? _data[id] : Faction.Neutral;
        }

        public void Set(int id, Faction value, bool overwrite = true)
        {
            if (_data.ContainsKey(id) && (!overwrite || _data[id] == value))
                return;

            _data[id] = value;
            Updated.Dispatch(id);
        }

        public bool IsSame(int id1, int id2)
        {
            return Get(id1) == Get(id2);
        }

        public bool IsOpposite(int id1, int id2)
        {
            return (int) Get(id1) == -(int) Get(id2) && Get(id1) != Faction.Neutral;
        }
    }
}