using model;
using utils.injection;
using UnityEngine;

namespace FSM.Unit.Speciality
{
    public class Damage : InjectableStateMachineBehaviour
    {
        [Inject]
        private TargetModel _target;
    
        [Inject]
        private HealthModel _health;
    
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            var target = _target.GetTarget(animator.gameObject.GetInstanceID());
            if (target != null)
                _health.Apply(target.GetInstanceID(), -1);
        
            animator.SetTrigger("ActionCompleted");
        }
    }
}
