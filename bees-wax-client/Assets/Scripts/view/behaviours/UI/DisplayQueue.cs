using System;
using model;
using UnityEngine;
using utils.injection;

namespace view.behaviours.UI
{
    public class DisplayQueue : InjectableBehaviour
    {
        [Inject] private UnitQueueModel _queue;

        public Transform container;

        public Transform random;

        private void Start()
        {
            Refresh();
            _queue.Updated.Add(Refresh);
        }

        private void Refresh()
        {
            foreach (Transform child in container)
                if (child != random)
                    Destroy(child.gameObject);

            foreach (var obj in _queue.Get())
            {
                var icon = (GameObject) Instantiate(Resources.Load($"Prefabs/UI/{obj.name}Icon"), container);
                icon.transform.localScale = Vector3.one / 2;
            }

            random.SetSiblingIndex(container.childCount - 1);
        }

        private void OnDestroy()
        {
            _queue.Updated.Remove(Refresh);
        }
    }
}