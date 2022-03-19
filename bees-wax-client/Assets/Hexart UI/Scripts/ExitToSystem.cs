using UnityEngine;

namespace Hexart_UI.Scripts
{
    public class ExitToSystem : MonoBehaviour
    {
        public void ExitGame()
        {
            Debug.Log("It's working :)");
            Application.Quit();
        }
    }
}