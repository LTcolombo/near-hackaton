using System;
using model;
using utils.injection;
using UnityEngine;

namespace FSM.Input
{
    public class TouchInputZoom : InjectableStateMachineBehaviour
    {
        [Inject]
        private InputModel _input;
        
        float _oldMagnitude;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            if (InputModel.ActiveTouchCount() >= 2)
            {
                var pinchVector = InputModel.GetActiveTouch(1).position - InputModel.GetActiveTouch(0).position;
                _oldMagnitude = pinchVector.magnitude;
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (InputModel.ActiveTouchCount() < 2) return; //this will be handled by base input behavious and exit state

            var magnitude = (InputModel.GetActiveTouch(1).position - InputModel.GetActiveTouch(0).position).magnitude;
            _input.Zoom += (magnitude - _oldMagnitude) / Math.Min(Screen.height, Screen.width);
            _oldMagnitude = magnitude;
        }
    }
}