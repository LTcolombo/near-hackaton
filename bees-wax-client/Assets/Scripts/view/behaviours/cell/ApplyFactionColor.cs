using System.Collections.Generic;
using System.Linq;
using model;
using model.data;
using utils.injection;
using UnityEngine;

namespace view.behaviours.cell
{
    public class ApplyFactionColor : InjectableBehaviour
    {
        [Inject]
        FactionModel _faction;
        
        [Inject]
        TargetModel _target;

        public static Dictionary<Faction, Color> FactionColors => new Dictionary<Faction, Color>
        {
            {Faction.Player, new Color(1f, 0.7f, 0.1f)},
            {Faction.Enemy, Color.white},
            {Faction.Neutral, Color.black}
        };

        public Renderer Renderer;
        public TrailRenderer Trail;
        public Light Light;

        private Material _material;
        private SpriteRenderer _sprite;

        void Start()
        {
            if (_material == null && Renderer is MeshRenderer)
                _material = ((MeshRenderer) Renderer).material;

            if (_sprite == null && Renderer is SpriteRenderer)
                _sprite = (SpriteRenderer) Renderer;
        
            OnFactionUpdated(gameObject.GetInstanceID());
        }

        void OnEnable()
        {
            _faction.Updated.Add(OnFactionUpdated);
            _target.Updated.Add(OnTargetUpdated);
        }

        void OnFactionUpdated(int id)
        {
            if (id != gameObject.GetInstanceID())
                return;
            
            UpdateColor();
        }
        
        void OnTargetUpdated(int id)
        {
            if (id != gameObject.GetInstanceID())
                return;
            
            UpdateColor();
        }

        private void UpdateColor()
        {
            var instanceID = gameObject.GetInstanceID();
            var target = _target.GetTarget(instanceID);
            var shouldBeSeen = _faction.Get(instanceID) == Faction.Player ||
                               target != null && _faction.Get(target.GetInstanceID()) == Faction.Player;

            var factionColor = FactionColors[_faction.Get(instanceID)];

            if (_material != null)
                _material.SetColor("_Color", factionColor);

            if (_sprite != null)
                _sprite.color = factionColor;

            if (Trail != null)
            {
                Trail.enabled = shouldBeSeen;
                var prevGradient = Trail.colorGradient;
                var gradient = new Gradient();
                gradient.SetKeys(
                    prevGradient.colorKeys.Select(e => new GradientColorKey(factionColor, e.time)).ToArray(),
                    prevGradient.alphaKeys);

                Trail.colorGradient = gradient;
            }

            if (Light != null)
            {
                Light.gameObject.SetActive(shouldBeSeen);
//                Light.color = factionColor;
            }
        }

        void OnDisable()
        {
            _faction.Updated.Remove(OnFactionUpdated);
            _target.Updated.Remove(OnTargetUpdated);
        }
    }
}