using System;
using model;
using model.data;
using utils.injection;
using UnityEngine.UI;

namespace view.behaviours.UI
{
    public class ShowRemainingResource : InjectableBehaviour
    {
        [Inject]
        ResourceModel _resources;

        private Text _text;

        void OnEnable()
        {
            UpdateValue(Faction.Neutral);
            _resources.Updated.Add(UpdateValue);
        }

        private void Update()
        {
            UpdateValue(Faction.Player);
        }

        void UpdateValue(Faction faction)
        {
            if (_text == null)
                _text = GetComponent<Text>();

            _text.text = ((int) _resources.remaining).ToString();
        }

        void OnDisable()
        {
            _resources.Updated.Remove(UpdateValue);
        }
    }
}