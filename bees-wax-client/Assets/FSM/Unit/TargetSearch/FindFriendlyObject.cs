using model;
using utils.injection;
using UnityEngine;

namespace FSM.Unit.TargetSearch
{
    public class FindFriendlyObject : FindTarget
    {
        [Inject]
        FactionModel _faction;
        
        protected override int GetLayer()
        {
            return LayerMask.GetMask("Cells");
        }

        protected override bool IsRelevant(GameObject owner, GameObject candidate)
        {
            //todo teams and is same/opposite
            return base.IsRelevant(owner, candidate) && _faction.IsSame(owner.GetInstanceID(), candidate.GetInstanceID()); 
        }
    }
}