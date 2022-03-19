using model;
using model.data;
using service;
using utils.injection;
using UnityEngine;

namespace FSM.Unit.TargetSearch
{
    public abstract class FindTarget : InjectableStateMachineBehaviour
    {
        [Inject]
        private TargetModel _targets;

        [Inject]
        private SelectionModel _selection;

        [Inject]
        private FactionModel _faction;
        
        [Inject]
        private MultiplayerService _multiplayer;
        
        public float Sight = 3;

        protected bool SelectedOnly;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            if (_faction.Get(animator.gameObject.GetInstanceID()) == Faction.Enemy)
                return;//todo target will come via network
            
            GameObject nearest = null;
            var nearestDistance = float.MaxValue;

            foreach (var hit in Physics.OverlapSphere(animator.transform.position, Sight, GetLayer()))
            {
                var collided = hit.gameObject;

                if (!IsRelevant(animator.gameObject, collided))
                    continue;

                var distance = (animator.transform.position - collided.transform.position).sqrMagnitude;
                if (!(distance < nearestDistance))
                    continue;

                nearestDistance = distance;
                nearest = collided;
            }
            
//todo synchonise
            if (nearest != null && _faction.Get(animator.gameObject.GetInstanceID()) == Faction.Player)
                _multiplayer.AssignTarget(animator.gameObject, nearest);
            
            if (nearest != null)
            {
                animator.SetTrigger("TargetFound");
                _targets.SetTarget(animator.gameObject.GetInstanceID(), nearest);
            }
            else
            {
                animator.SetTrigger("NoTargetFound");
                _targets.SetTarget(animator.gameObject.GetInstanceID(), null);
            }
        }

        protected abstract int GetLayer();

        protected virtual bool IsRelevant(GameObject owner, GameObject candidate)
        {
            return !SelectedOnly ||
                   _selection.IsSelected(
                       _faction.Get(owner.GetInstanceID()),
                       candidate.GetInstanceID());
        }
    }
}