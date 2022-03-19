using UnityEngine;

namespace Hexart_UI.Scripts
{
    public class LoadingStyle : MonoBehaviour
    {
        public void SetStyle(string prefabToLoad)
        {
            LoadingScreen.prefabName = prefabToLoad;
        }
    }
}