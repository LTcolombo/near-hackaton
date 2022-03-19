using model;
using utils.injection;
using UnityEngine;

namespace FSM.Unit.Speciality
{
    public class CollectResource : InjectableStateMachineBehaviour
    {   
        [Inject]
        private CollectionModel _collected;
        
        [Inject]
        private TargetModel _target;
        
        public int Limit;
        public float Rate;
        
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var target = _target.GetTarget(animator.gameObject.GetInstanceID());

            if (target == null) return;
            
            var source = target.GetComponent<Source>();

            if (source == null) return; //this means we are transitioning to a different state and target is no onger a source
            
            var toCollect = Rate * Time.deltaTime;
            if (source.Reduce(toCollect))
            {
                _collected.Inc(toCollect);
                if (_collected.Get() >= Limit)
                    animator.SetTrigger("ActionCompleted");
            }
            else
            {
                animator.SetTrigger("ActionCompleted");
            }
        }
    }
}