using UnityEngine;
using utils.signal;

namespace view.behaviours.UI
{
    public class FocusOn : MonoBehaviour
    {
        public static readonly Signal<GameObject> NeedsFocus = new Signal<GameObject>();

        private void Start()
        {
            NeedsFocus.Add(FocusOnTarget);
        }

        private void FocusOnTarget(GameObject target)
        {
            transform.position = target.transform.position.normalized * transform.position.magnitude;
            transform.LookAt(Vector3.zero);
        }

        private void OnDestroy()
        {
            NeedsFocus.Remove(FocusOnTarget);
        }
    }
}
