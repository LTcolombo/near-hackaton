using utils.customUI;
using utils.injection;

namespace model
{
    
    public enum MovementType
    {
        Wander,
        Harvest,
        Direct
    }
    
    [PerNamedObject]
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MoveParamsModel
    {
        public MovementType Type;
        public float Velocity;
        public MinMaxRange Inertia;

        public void SetMoveParams(MovementType type, float velocity, MinMaxRange inertia)
        {
            Type = type;
            Velocity = velocity;
            Inertia = inertia;
        }
    }
}