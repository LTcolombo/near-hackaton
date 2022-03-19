using System.Collections.Generic;
using model.data;
using utils.injection;
using utils.signal;

namespace model
{
    [Singleton]
    public class ResourceModel
    {
        private readonly Dictionary<Faction, float> _data = new Dictionary<Faction, float>
        {
            {Faction.Player, 0},
            {Faction.Enemy, 0}
        };

        public Signal<Faction> Updated { get; } = new Signal<Faction>();

        public float remaining;

        public float total;

        public int sourceCount => _sourceCount;
        private int _sourceCount;

        public void Set(Faction faction, float value)
        {
            _data[faction] = value;
            Updated.Dispatch(faction);
        }

        public float Get(Faction faction)
        {
            return _data[faction];
        }

        public void RegisterSource()
        {
            _sourceCount++;
        }
    }
    
}