using UnityEngine;
using UnityEngine.UI;

namespace view.behaviours.UI
{
    public class MoveUpAndFade : MonoBehaviour
    {
        public float Duration = 1;
        public Vector3 Offset = Vector3.up * 50; //px!

        private Text _text;
        private Vector3 _pos;
        private float _timePassed;

        private void Start()
        {
            _text = GetComponent<Text>();
        }

        public void Anchor(Vector3 value)
        {
            _pos = value;
        }
        
        void Update()
        {
            if (_text == null)
                return;
            
            if (_timePassed > Duration){
                Destroy(gameObject);
                return;
            }

            var pos = transform.position;
            if (_pos != default(Vector3))
                pos = Camera.main.WorldToScreenPoint(_pos);
            transform.position = pos + Offset * (_timePassed / Duration);

            var color = _text.color;
            _text.color = new Color(color.r, color.g, color.b, Duration - (_timePassed / Duration));

            _timePassed += Time.deltaTime;
        }
    }
}