using System;

namespace Modifiers
{
    [Serializable]
    public class SpeedModification : Modification
    {
        public float boost;
        public float multiplier = 1;

        public override void Apply(Stats stats)
        {
            stats.speed += boost;
            stats.speed *= multiplier;
        }

        public override void Unapply(Stats stats)
        {
            stats.speed /= multiplier;
            stats.speed -= boost;
        }
    }
}