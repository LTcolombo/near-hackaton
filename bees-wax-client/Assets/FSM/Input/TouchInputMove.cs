using model;
using utils.injection;
using UnityEngine;

namespace FSM.Input
{
    public class TouchInputMove : InjectableStateMachineBehaviour
    {
        [Inject]
        protected InputModel _input;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            _input.RegisterDrag();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
            _input.RegisterDrag();
        }
    }
}