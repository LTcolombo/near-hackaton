using System.Collections;
using model;
using model.data;
using utils.injection;
using UnityEngine;

namespace view.behaviours.cell
{
    public class ApplySelectionEffect : InjectableBehaviour
    {
        [Inject]
        SelectionModel _selection;

        public Renderer Renderer;
        private Material _material;
        private Color _selectionColor;

        IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            
            if (_material == null && Renderer is MeshRenderer)
                _material = ((MeshRenderer) Renderer).material;

            UpdateValue(Faction.Player, gameObject.GetInstanceID());
        }

        void OnEnable()
        {
            _selection.Updated.Add(UpdateValue);
        }

        int CompareColors(Color c1, Color c2)
        {
            if (c1.r <= c2.r && c1.g <= c2.g && c1.b <= c2.b)
                return -1;

            if (c1.r >= c2.r && c1.g >= c2.g && c1.b >= c2.b)
                return 1;

            return 0;
        }

        void Update()
        {
            if (_material == null) return;
            if (_selectionColor == Color.black) return;

            var currentColor = _material.GetColor("_EmissionColor");

            if (currentColor.r <= 0 && currentColor.g <= 0 && currentColor.b <= 0){
                _material.SetColor("_EmissionColor", _selectionColor);
                return;
            }

            var newColor = currentColor - Time.deltaTime * _selectionColor * 2;
            _material.SetColor("_EmissionColor", newColor);
        }

        void UpdateValue(Faction faction, int id)
        {
            if (faction != Faction.Player)
                return;
                
            if (id != gameObject.GetInstanceID())
                return;

            if (_material == null) return;
            
            _selectionColor = _selection.IsSelected(Faction.Player, id)
                ? ApplyFactionColor.FactionColors[Faction.Player] / 20
                : Color.black;
                
            _material.SetColor("_EmissionColor", _selectionColor);
        }

        void OnDisable()
        {
            _selection.Updated.Remove(UpdateValue);
        }
    }
}