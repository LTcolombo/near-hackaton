using UnityEngine;

namespace utils.injection
{
    public class InjectableStateMachineBehaviour : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Injector.Instance.Resolve(this, animator.gameObject.GetInstanceID());
        }
    }
}