using System.Collections;
using model;
using utils.injection;
using UnityEngine;

namespace view.behaviours.unit
{
    public class RegisterUnitType : InjectableBehaviour
    {
        [Inject]
        UnitRegisterModel _units;
        
        [Inject]
        FactionModel _faction;

        private string _prefabName;

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            
            _prefabName = name.Split(' ')[0].Split('(')[0];
            _units.Add(_faction.Get(gameObject.GetInstanceID()), _prefabName);
        }

        private void OnDestroy()
        {
            if (_prefabName != null)
            {
                _units.Remove(_faction.Get(gameObject.GetInstanceID()), _prefabName);
            }
        }
    }
}