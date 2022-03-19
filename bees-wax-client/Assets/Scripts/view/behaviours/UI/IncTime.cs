using UnityEngine;
using UnityEngine.UI;

namespace view.behaviours.UI
{
    public class IncTime : MonoBehaviour
    {
        public Text Text;
        private float _enabledAt;

        // Start is called before the first frame update
        void OnEnable()
        {
            _enabledAt = Time.realtimeSinceStartup;
        }

        // Update is called once per frame
        void Update()
        {
            var timeSinceEnabled = Time.realtimeSinceStartup - _enabledAt;
            var ms = $"{(int) (1000 * (timeSinceEnabled - (int) timeSinceEnabled))}".PadRight(3, '0');

            var m = (int) timeSinceEnabled / 60;
            var s = (int) timeSinceEnabled % 60;
            Text.text = m > 0 ? $"{m}:{s}:{ms}" : $"{s}:{ms}";
        }
    }
}