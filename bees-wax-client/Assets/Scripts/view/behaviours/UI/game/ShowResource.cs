using System;
using model;
using model.data;
using UnityEngine;
using utils.injection;
using UnityEngine.UI;

namespace view.behaviours.UI
{
    public class ShowResource : InjectableBehaviour
    {
        [Inject]
        ResourceModel _resources;

        [Inject]
        FactionModel _factionModel;

        public Faction Faction;
        public RectTransform Bar;
        public float MaxScale = 42f;
        
        public Text text;

        void OnEnable()
        {
            _resources.Updated.Add(UpdateValue);
            UpdateValue(Faction);
        }

        void UpdateValue(Faction updatedFaction)
        {
            if (updatedFaction != Faction)
                return;

            if (text == null)
                text = GetComponent<Text>();

            text.text = ((int) _resources.Get(Faction)).ToString();
            
            var scale = (float)Math.Max(0.5f, MaxScale * _resources.Get(Faction) / _resources.total);
            Bar.sizeDelta = new Vector2(scale , Bar.sizeDelta.y);
        }

        void OnDisable()
        {
            _resources.Updated.Remove(UpdateValue);
        }
    }
}