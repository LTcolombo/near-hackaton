using utils.injection;
using model;
using model.data;
using UnityEngine;

namespace view.input.desktop
{
	public class ClickHandler : InjectableBehaviour 
	{
		[Inject]
		SelectionModel _selection;
		
		[Inject]
		FactionModel _faction;
		
		// Update is called once per frame
		private void Update()
		{
			if (!Application.isMobilePlatform)
			{
				if (Input.GetMouseButtonUp(0))
				{
					var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
	}
}
