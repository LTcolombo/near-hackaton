using utils.injection;
using model;
using UnityEngine;

namespace view.input.desktop
{
    public class ZoomHandler : InjectableBehaviour
    {
        [Inject]
        InputModel _input;
        
        // Update is called once per frame
        private void Update()
        {
            if (!Application.isMobilePlatform)
            {
                _input.Zoom += Input.mouseScrollDelta.y / 100;
            }
        }
    }
}