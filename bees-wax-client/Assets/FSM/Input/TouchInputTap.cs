using model;
using model.data;
using utils.injection;
using UnityEngine;

namespace FSM.Input
{
    public class TouchInputTap : InjectableStateMachineBehaviour
    {
        [Inject]
        SelectionModel _selection;

        [Inject]
        InputModel _input;

        [Inject]
        FactionModel _faction;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            var ray = Camera.main.ScreenPointToRay(_input.LastTouch.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, ray.origin.magnitude))
            {
                var hitId = hit.collider.gameObject.GetInstanceID();
                if (_faction.Get(hitId) == Faction.Neutral)
                {
                    _selection.ToggleSelection(Faction.Player, hitId);
                }
            }
        }
    }
}