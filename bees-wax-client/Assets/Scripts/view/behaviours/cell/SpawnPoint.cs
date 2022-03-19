using System.Collections;
using System.Linq;
using model;
using model.data;
using service;
using utils.injection;
using UnityEngine;
using view.behaviours.UI;

namespace view.behaviours.cell
{
    public class SpawnPoint : InjectableBehaviour
    {
        public GameObject[] StartUnits;

        [Inject]
        private MultiplayerService _multiplayer;
    
        [Inject]
        private FactionModel _faction;

        public int Id;

        protected override void Awake()
        {
            base.Awake();
            GetComponent<SpriteRenderer>().enabled = false; //hide in runtime
        }

        private IEnumerator Start()
        {
            if (_multiplayer.GetPlayerIndex() == Id)
            {
                StartUnits.ToList().ForEach(u =>
                {
                    _multiplayer.Create(Instantiate(u, transform.position, Quaternion.identity, transform.parent.parent));
                });
            
                yield return new WaitForFixedUpdate();
                
                _multiplayer.AssignFaction(transform.parent.gameObject.GetInstanceID());
                _faction.Set(transform.parent.gameObject.GetInstanceID(), Faction.Player);
                
                
                yield return new WaitForFixedUpdate();
                FocusOn.NeedsFocus.Dispatch(gameObject);
            }

            Destroy(gameObject);
        }
    }
}
