using utils.injection;
using model;
using UnityEngine;

namespace view.input.desktop
{
    public class DragHandler : InjectableBehaviour
    {
        [Inject]
        InputModel _input;
        
        private bool _dragging;
        private Vector2 _oldPos;

     
        private void Update()
        {
            if (!Application.isMobilePlatform)
            {
                if (Input.GetMouseButton(0))
                {
                    if (!_dragging)
                        StartDragAt(Input.mousePosition);
                    else
                        DragTo(Input.mousePosition);
                }
                else if (_dragging)
                {
                    StopDrag();
                }
            }
        }

        private void StartDragAt(Vector2 point)
        {
            _oldPos = point;
            _dragging = true;
        }

        private void DragTo(Vector2 point)
        {
            _input.DragDelta = point - _oldPos;
            _oldPos = point;
        }

        private void StopDrag()
        {
            _dragging = false;
        }
    }
}