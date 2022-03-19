using System.Collections.Generic;
using model.data;
using utils.injection;
using utils.signal;

namespace model
{
    [Singleton]
    public class SelectionModel
    {
        private readonly Dictionary<Faction, Dictionary<int, bool>> _data =
            new Dictionary<Faction, Dictionary<int, bool>>();

        public Signal<Faction, int> Updated { get; } = new Signal<Faction, int>();

        public void ToggleSelection(Faction faction, int id)
        {
            if (!_data.ContainsKey(faction))
                _data[faction] = new Dictionary<int, bool>();

            _data[faction][id] = !_data[faction].ContainsKey(id) || !_data[faction][id];
            Updated.Dispatch(faction, id);
        }

        public bool IsSelected(Faction faction, int id)
        {
            if (!_data.ContainsKey(faction))
                _data[faction] = new Dictionary<int, bool>();

            return _data[faction].ContainsKey(id) && _data[faction][id];
        }
    }
}