using UnityEngine;

namespace FSM.Unit.Common
{
    public class RefreshTargetIn : StateMachineBehaviour
    {
        private float _accumulated;

        public float Time = 1f;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _accumulated = 0f;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _accumulated += UnityEngine.Time.deltaTime;
            if (_accumulated >= Time) 
                animator.SetTrigger("RefreshTarget");
        }
    }
}