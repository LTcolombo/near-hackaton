using UnityEngine;

namespace Hexart_UI.Scripts
{
    public class UIElementInFront : MonoBehaviour
    {
        void Start()
        {
            this.transform.SetAsFirstSibling();
        }
    }
}