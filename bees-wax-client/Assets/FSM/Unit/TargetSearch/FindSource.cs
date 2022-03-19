using UnityEngine;

namespace FSM.Unit.TargetSearch
{
    public class FindSource : FindTarget
    {
        protected override int GetLayer()
        {
            return LayerMask.GetMask("Default");
        }

        protected override bool IsRelevant(GameObject owner, GameObject candidate)
        {
            return base.IsRelevant(owner, candidate) && candidate.CompareTag("Source");
        }
    }
}