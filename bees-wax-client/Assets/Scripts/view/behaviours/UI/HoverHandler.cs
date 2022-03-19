using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace view.behaviours.UI
{
    public class HoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public RectTransform[] ObjectsToRotate;
        public Text Label;

        private float _targetRotation = 0;
        private float _currentRotation = 0;

        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            _targetRotation = -60f;
            if (Label != null)
                Label.color = Color.green;
        }

        //Detect when Cursor leaves the GameObject
        public void OnPointerExit(PointerEventData pointerEventData)
        {
            _targetRotation = 0f;
            if (Label != null)
                Label.color = Color.white;
        }

        private void Update()
        {
            if (Math.Abs(_currentRotation - _targetRotation) < 0.0001f)
                return;

            _currentRotation += (_targetRotation - _currentRotation) * Time.deltaTime * 10;

            foreach (var obj in ObjectsToRotate)
                obj.rotation = Quaternion.Euler(0f, 0f, _currentRotation);
        }
    }
}