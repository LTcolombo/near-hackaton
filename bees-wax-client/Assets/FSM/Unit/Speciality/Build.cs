using model;
using model.data;
using service;
using utils.injection;
using UnityEngine;

namespace FSM.Unit.Speciality
{
	public class Build : ContiniousAction
	{
		[Inject]
		private SelectionModel _selection;

		[Inject]
		private MultiplayerService _multiplayer;
		
		[Inject]
		private NFTSelectionService _nftSelection;
		
		protected override ActionType _actionType => ActionType.Build;

		protected override void OnComplete(GameObject owner, Faction ownerFaction)
		{
			
//todo synchonise
			_multiplayer.AssignFaction(_id);
			_faction.Set(_id, ownerFaction);

			if (_selection.IsSelected(ownerFaction, _id))
				_selection.ToggleSelection(ownerFaction, _id);
		}
		
		protected override bool IsComplete(Faction objFaction)
		{
			return objFaction == _faction.Get(_id);
		}
		
		protected override float DeltaTime()
		{
			return Time.deltaTime * _nftSelection?.Effect?.build ?? 1;
		}
	}
}
