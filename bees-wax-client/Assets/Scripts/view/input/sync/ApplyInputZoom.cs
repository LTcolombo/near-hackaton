using System;
using model;
using utils.injection;
using UnityEngine;

namespace view.input.sync
{
    [Serializable]
    public struct ZoomDescriptor
    {
        public Quaternion Rotation;
        public Vector3 Position;
    }

    public class ApplyInputZoom : InjectableBehaviour
    {
        [Inject]
        InputModel _input;
        
        public ZoomDescriptor MaxZoom;
        public ZoomDescriptor MinZoom;
        
        private void Update()
        {
            transform.localRotation = Quaternion.Lerp(MinZoom.Rotation, MaxZoom.Rotation, Mathf.Sqrt(_input.Zoom));
            transform.localPosition = MinZoom.Position * (1 - _input.Zoom) + MaxZoom.Position * Mathf.Sqrt(_input.Zoom);

            GetComponent<Camera>().farClipPlane = transform.position.magnitude;
        }
    }
}