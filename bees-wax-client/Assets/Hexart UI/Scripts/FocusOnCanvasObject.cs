using UnityEngine;
using UnityEngine.UI;

namespace Hexart_UI.Scripts
{
    public class FocusOnCanvasObject : MonoBehaviour
    {
        private GameObject focusedObj;

        public void FocusButton(Button focusedObject)
        {
            focusedObject.Select();
            focusedObj = focusedObject.GetComponent<GameObject>();
        //    EventSystem.current.SetSelectedGameObject(focusedObj, null);
        }

        public void FocusIP(InputField inputFieldSelect)
        {
            inputFieldSelect.Select();
            focusedObj = inputFieldSelect.GetComponent<GameObject>();
        //    EventSystem.current.SetSelectedGameObject(focusedObj, null);
        }

        public void FocusToggle(Toggle toggleSelect)
        {
            toggleSelect.Select();
            focusedObj = toggleSelect.GetComponent<GameObject>();
        //    EventSystem.current.SetSelectedGameObject(focusedObj, null);
        }

        public void FocusScrollBar(Scrollbar scrollSelect)
        {
            scrollSelect.Select();
            focusedObj = scrollSelect.GetComponent<GameObject>();
        //    EventSystem.current.SetSelectedGameObject(focusedObj, null);
        }
    }
}