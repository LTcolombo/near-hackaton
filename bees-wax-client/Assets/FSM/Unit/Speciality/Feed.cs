using model;
using model.data;
using service;
using utils.injection;
using UnityEngine;

namespace FSM.Unit.Speciality
{
    public class Feed : ContiniousAction
    {
        [Inject]
        private LayedEggsModel _eggs;
        
        [Inject]
        private MultiplayerService _multiplayer;
        
        [Inject]
        private NFTSelectionService _nftSelection;

        protected override ActionType _actionType => ActionType.Feed;

        protected override void OnComplete(GameObject owner, Faction ownerFaction)
        {
            //check  other possible reasons of Contribute returning false except competion
            if (!_eggs.HasLayedEgg(_id)) return;
            
            var obj = _eggs.GetLayedEgg(_id);
            
            //todo synchonise create
            _multiplayer.Create(obj);
            obj.SetActive(true);
            _eggs.RemoveLayedEgg(_id);
        }
		
        protected override bool IsComplete(Faction objFaction)
        {
            return !_eggs.HasLayedEgg(_id);
        }
        
        protected override float DeltaTime()
        {
            return Time.deltaTime * _nftSelection?.Effect?.feed ?? 1;
        }
    }
}