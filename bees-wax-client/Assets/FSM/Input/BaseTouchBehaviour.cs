using model;
using utils.injection;
using UnityEngine;

namespace FSM.Input
{
    public class BaseTouchBehaviour : InjectableStateMachineBehaviour
    {
        [Inject]
        private InputModel _input;
        
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            animator.SetFloat("Time", 0);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _input.MakeSnapshot();
                
            animator.SetFloat("Time", animator.GetFloat("Time") + Time.deltaTime);
            animator.SetInteger("TouchCount", InputModel.ActiveTouchCount());
            animator.SetBool("Moved", !_input.LastTouch.deltaPosition.Equals(default(Vector2)));
        }
    }
}