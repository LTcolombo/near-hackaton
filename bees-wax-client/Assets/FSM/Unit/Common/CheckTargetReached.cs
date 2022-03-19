using model;
using utils.injection;
using UnityEngine;

namespace FSM.Unit.Common
{
    public class CheckTargetReached : InjectableStateMachineBehaviour
    {
        [Inject]
        TargetModel _targets;


        private Animator _animator;
        private Collider _targetCollider;
        private LayerMask _layerMask;
        private bool _enabled;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            var target = _targets.GetTarget(animator.gameObject.GetInstanceID());
            if (target != null)
            {
                _targetCollider = target.GetComponent<Collider>();
                _layerMask = 1 << _targetCollider.gameObject.layer;
                _enabled = true;
            }
            else
            {
                animator.SetTrigger("NoTargetFound");
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!_enabled)
                return;

            if (_targetCollider == null)
            {
                animator.SetTrigger("NoTargetFound");
                return;
            }

//            RaycastHit hit;
//            var pos = animator.transform.position;
//            
//            Debug.DrawRay(pos * 1.5f, -pos);
//            if (!Physics.Raycast(new Ray(pos * 1.5f, -pos), out hit, pos.magnitude, _layerMask))
//                return;
//            if (hit.collider != _targetCollider)
//                return;

            foreach (var hit in Physics.OverlapCapsule(animator.transform.position, Vector3.zero, 0.1f, _layerMask))
            {
                if (hit != _targetCollider)
                    continue;

                animator.SetTrigger("TargetReached");
                _enabled = false;
            }
        }
    }
}