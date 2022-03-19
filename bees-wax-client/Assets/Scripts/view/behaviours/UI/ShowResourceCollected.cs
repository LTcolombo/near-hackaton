using model;
using utils.injection;
using UnityEngine;
using UnityEngine.UI;

namespace view.behaviours.UI
{
    public class ShowResourceCollected : InjectableBehaviour
    {
        [Inject]
        ResourceModel _resources;

        [Inject]
        FactionModel _factionModel;

        public GameObject TextPrefab;

        private static ShowResourceCollected _ref;
        protected override void Awake()
        {
            base.Awake();
            _ref = this;
        }

        public static void ShowCollectionEffect(int value, Vector3 position)//oh no, static TODO
        {
            //ignore if on the other side of the hive
            
            var cameraPlane = new Plane(Camera.main.transform.position, 0);
            if (!cameraPlane.GetSide(position))
                return;
            
            var text = Instantiate(_ref.TextPrefab, position, Quaternion.identity, _ref.transform);
            text.GetComponent<MoveUpAndFade>().Anchor(position);
            text.GetComponent<Text>().text = "+" + value;
        }
    }
}