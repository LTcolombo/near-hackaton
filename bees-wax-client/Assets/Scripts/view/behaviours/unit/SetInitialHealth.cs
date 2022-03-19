using model;
using utils.injection;

namespace view.behaviours.unit
{
    public class SetInitialHealth : InjectableBehaviour
    {
        [Inject]
        HealthModel _health;

        public int Value;

        private void Start()
        {
            _health.Set(gameObject.GetInstanceID(), Value);
        }
    }
}