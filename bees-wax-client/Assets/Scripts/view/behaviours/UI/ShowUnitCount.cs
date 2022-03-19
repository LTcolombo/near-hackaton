using model;
using model.data;
using utils.injection;
using UnityEngine;
using UnityEngine.UI;

namespace view.behaviours.UI
{
    public class ShowUnitCount : InjectableBehaviour
    {
        public GameObject UnitPrefab;
        
        [Inject]
        UnitRegisterModel _unitRegister;
        
        [Inject]
        FactionModel _faction;

        private Text _text;

        void OnEnable()
        {
            _unitRegister.Updated.Add(UpdateValue);
            UpdateValue(Faction.Player, UnitPrefab.name);
        }

        void UpdateValue(Faction faction, string type)
        {
            if (faction != Faction.Player)
                return;
            
            if (UnitPrefab.name != type)
                return;
            
            if (_text == null)
                _text = GetComponent<Text>();
            
            _text.text = _unitRegister.Get(Faction.Player, UnitPrefab.name).ToString();
        }

        void OnDisable()
        {
            _unitRegister.Updated.Remove(UpdateValue);
        }
    }
}