using UnityEngine;

namespace Hexart_UI.Scripts
{
    public class LoadScene : MonoBehaviour
    {
        public void ChangeToScene(string sceneName)
        {
            LoadingScreen.LoadScene(sceneName);
        }
    }
}