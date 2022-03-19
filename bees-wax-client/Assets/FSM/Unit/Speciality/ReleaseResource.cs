using api;
using model;
using model.data;
using utils.injection;
using UnityEngine;
using view.behaviours.UI;

namespace FSM.Unit.Speciality
{
	public class ReleaseResource : InjectableStateMachineBehaviour {

		[Inject]
		ResourceModel _resources;
        
		[Inject]
		FactionModel _faction;
        
		[Inject]
		CollectionModel _collected;
        
		[Inject]
		EscrowApi _escrow;
        
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);


			var value = _collected.Get();
			if (value > 0)
			{
				_collected.Reset();

				var faction = _faction.Get(animator.gameObject.GetInstanceID());
				if (faction == Faction.Player)
				{
					//todo static access
					ShowResourceCollected.ShowCollectionEffect((int) value, animator.gameObject.transform.position);
					_escrow.SubmitResource(value);
				}
			}
			animator.SetTrigger("ActionCompleted");
		}
	}
}
