using model;
using utils.injection;
using UnityEngine;

namespace FSM.Unit.TargetSearch
{
    public class FindEnemyUnit : FindTarget
    {
        [Inject]
        FactionModel _faction;
        
        protected override int GetLayer()
        {
            return LayerMask.GetMask("Units");
        }

        protected override bool IsRelevant(GameObject owner, GameObject candidate)
        {
            return _faction.IsOpposite(owner.GetInstanceID(), candidate.GetInstanceID()); 
        }
    }
}