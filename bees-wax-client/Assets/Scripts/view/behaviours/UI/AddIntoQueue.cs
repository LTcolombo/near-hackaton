using System;
using model;
using model.data;
using utils.injection;
using UnityEngine;
using UnityEngine.EventSystems;

namespace view.behaviours.UI
{
    public class AddIntoQueue : InjectableBehaviour, IPointerClickHandler
    { 
        public GameObject UnitPrefab;
        
        [Inject]
        UnitQueueModel _queue;

        private void Start()
        {
            _queue.Register(UnitPrefab);
        }

        public void OnPointerClick(PointerEventData eventData)
        {        
            _queue.Push(UnitPrefab);
        }
    }
}