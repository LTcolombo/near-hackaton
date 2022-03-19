using System;
using model;
using model.data;
using service;
using utils.injection;
using UnityEngine;

namespace view.behaviours.unit
{
    public class Age : InjectableBehaviour
    {
        [Inject] UnitRegisterModel _register;

        [Inject] FactionModel _faction;

        [Inject] private MultiplayerService _multiplayer;

        public float Lifetime = 100f;

        private float _accumulated;
        private string _type;

        private void Start()
        {
            _type = name.Split(' ')[0].Split('(')[0];
        }

        private void Update()
        {
            var faction = _faction.Get(gameObject.GetInstanceID());
            
            if (faction == Faction.Enemy)
                return; //todo generalise enemy synchro-actions being not simulated

            if (_register.Get(faction, _type) < 2)
                return;
            
            if (Lifetime < 0)
            {
                Destroy(gameObject);
                _multiplayer.Destroy(gameObject);
                return;
            }
            
            Lifetime -= Time.deltaTime;//todo NFT?
        }
    }
}