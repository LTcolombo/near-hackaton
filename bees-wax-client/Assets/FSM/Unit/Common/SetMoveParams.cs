using model;
using utils.customUI;
using utils.injection;
using UnityEngine;

namespace FSM.Unit.Common
{

	public class SetMoveParams : InjectableStateMachineBehaviour {

		public MovementType Type = MovementType.Wander;
		
		[MinMaxRange (0f, 30f)]
		public MinMaxRange Inertia;
		
		[Range(0f, 10f)]
		public float Velocity = 5;
		
		[Inject]
		MoveParamsModel _moveParams;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			base.OnStateEnter(animator, stateInfo, layerIndex);
			_moveParams.SetMoveParams(Type, Velocity, Inertia);
		}
	}
}
