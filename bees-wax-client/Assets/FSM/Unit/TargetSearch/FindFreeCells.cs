using model;
using utils.injection;
using UnityEngine;

namespace FSM.Unit.TargetSearch
{
    public class FindFreeCells : FindFriendlyObject
    {
        [Inject]
        LayedEggsModel _eggs;

        [Inject]
        FactionModel _faction;

        public bool Occupied;
        
        protected override bool IsRelevant(GameObject owner, GameObject candidate)
        {
            if (!base.IsRelevant(owner, candidate))
                return false;

            return Occupied == _eggs.HasLayedEgg(candidate.GetInstanceID());
        }
    }
}