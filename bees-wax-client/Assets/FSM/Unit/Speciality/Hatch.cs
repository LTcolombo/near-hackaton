using model;
using model.data;
using service;
using utils.injection;
using UnityEngine;

namespace FSM.Unit.Speciality
{
    
    public class Hatch : ContiniousAction
    {
        [Inject]
        private LayedEggsModel _eggs;
        
        [Inject]
        private UnitQueueModel _queue;
        
        [Inject]
        private NFTSelectionService _nftSelection;

        private GameObject _currentTarget;

        protected override ActionType _actionType => ActionType.Hatch;

        private void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, animatorStateInfo, layerIndex);
            _currentTarget = _target.GetTarget(animator.gameObject.GetInstanceID());
        }

        protected override void OnComplete(GameObject owner, Faction ownerFaction)
        {
            var egg = Instantiate(_queue.Roll(), 
                _currentTarget.transform.position, Quaternion.identity, owner.transform.parent); 
                
            _faction.Set(egg.GetInstanceID(), _faction.Get(_currentTarget.GetInstanceID()));
            egg.SetActive(false);
                
            _eggs.SetLayedEgg(_currentTarget.GetInstanceID(), egg);
        }
		
        protected override bool IsComplete(Faction objFaction)
        {
            return _eggs.HasLayedEgg(_id);
        }
        
        protected override float DeltaTime()
        {
            return Time.deltaTime * _nftSelection?.Effect?.hatch ?? 1;
        }
    }
}