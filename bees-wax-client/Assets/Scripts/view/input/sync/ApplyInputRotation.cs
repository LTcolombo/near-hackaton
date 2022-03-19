using model;
using utils.injection;
using UnityEngine;

namespace view.input.sync
{
    public class ApplyInputRotation : InjectableBehaviour
    {
        [Inject]
        InputModel _input;
        
        private void Update()
        {
            var screenSize = Mathf.Max(Screen.height, Screen.width);
            if (_input.DragDelta == Vector2.zero)
                return;
            
            var dragDelta = _input.DragDelta/ (1 + _input.Zoom);
            transform.RotateAround(Vector3.zero, transform.up, dragDelta.x * 180 / screenSize);
            transform.RotateAround(Vector3.zero, transform.right, -dragDelta.y * 180 / screenSize);
            _input.DragDelta *= 0.8f;
            
            if (_input.DragDelta.SqrMagnitude()<0.001f)
                _input.DragDelta = Vector2.zero;
        }
    }
}