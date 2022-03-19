using model;
using model.data;
using service;
using UnityEngine;
using utils.injection;

namespace view.behaviours.unit
{
    public class ApplyHealth : InjectableBehaviour
    {
        [Inject]
        private HealthModel _health;
    
        [Inject]
        private FactionModel _faction;
        
        [Inject]
        private MultiplayerService _multiplayer;

        // Update is called once per frame
        void Update()
        {
            var health = _health.Get(gameObject.GetInstanceID());
            if (health <= 0 && _faction.Get(gameObject.GetInstanceID()) != Faction.Enemy)//todo generalise enemy symchro-actions being not simulated)
            {
                Die();
            }
            else
            {
                var tail = GetComponent<TrailRenderer>();
                if (tail != null)
                {
                    tail.time = health * 0.5f;
                }
            }
        }

        private async void Die()
        {
            Destroy(gameObject); 
            await _multiplayer.Destroy(gameObject);
        }
    }
}