using model;
using model.data;
using utils.injection;

namespace view.behaviours.cell
{
    public class InitialFaction : InjectableBehaviour
    {
        [Inject]
        FactionModel _faction;

        public Faction Value = Faction.Neutral;

        void Start()
        {
            _faction.Set(gameObject.GetInstanceID(), Value, false);
            Destroy (this);
        }
    }
}