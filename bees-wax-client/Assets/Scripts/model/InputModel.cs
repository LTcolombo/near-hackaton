using System;
using utils.injection;
using UnityEngine;

namespace model
{
    [Singleton]
    public class InputModel
    {
        public Vector2 DragDelta;

        private Touch _lastTouch;

        public Touch LastTouch => _lastTouch;

        float _zoom;

        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = Math.Min(1, Math.Max(0, value)); }
        }

        public void MakeSnapshot()
        {
            if (Input.touchCount > 0)
                _lastTouch = Input.touches[0];
        }

        public static int ActiveTouchCount()
        {
            return Input.touchCount;
        }

        public static Touch GetActiveTouch(int index)
        {
            return Input.touches[index];
        }

        public void RegisterDrag()
        {
            if (ActiveTouchCount() > 0)
                DragDelta = _lastTouch.deltaPosition;
        }
    }
}