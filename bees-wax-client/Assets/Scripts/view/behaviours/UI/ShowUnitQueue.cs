using model;
using model.data;
using utils.injection;
using UnityEngine;

namespace view.behaviours.UI
{
    public class ShowUnitQueue : InjectableBehaviour
    {
        [Inject]
        UnitQueueModel _queue;

        void OnEnable()
        {
            _queue.Updated.Add(UpdateValue);
        }

        void UpdateValue()
        {
            
        }

        void OnDisable()
        {
            _queue.Updated.Remove(UpdateValue);
        }
    }
}