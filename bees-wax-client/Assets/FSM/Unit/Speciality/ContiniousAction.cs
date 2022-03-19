using model;
using model.data;
using utils.injection;
using UnityEngine;

namespace FSM.Unit.Speciality
{
	public abstract class ContiniousAction : InjectableStateMachineBehaviour
	{

		[Inject]
		private ActionProgressModel _progress;
		
		[Inject]
		protected FactionModel _faction;
	
		[Inject]
		protected TargetModel _target;
        

		protected abstract ActionType _actionType { get; }

		protected int _id;
	 
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);
			var target = _target.GetTarget(animator.gameObject.GetInstanceID());
			if (target == null)
			{
				_id = 0;
				animator.SetTrigger("ActionCompleted");
				return;
			}

			_id = target.GetInstanceID();
		}
	
		// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateUpdate(animator, stateInfo, layerIndex);
			if (_id == 0)
				return;
		
			var objFaction = _faction.Get(animator.gameObject.GetInstanceID());
			
			//todo return if slave
			if (objFaction == Faction.Enemy)
				return;
			
			if (!_progress.Contribute(_id, objFaction, _actionType, DeltaTime()))
			{
				OnComplete(animator.gameObject, objFaction);
			}
				
			if (IsComplete(objFaction))
			{
				animator.SetTrigger("ActionCompleted");
				_id = 0;
			}
			
		}

		protected virtual float DeltaTime()
		{
			return Time.deltaTime;
		}

		protected abstract bool IsComplete(Faction objFaction);

		protected abstract void OnComplete(GameObject owner, Faction ownerFaction);
	}
}
