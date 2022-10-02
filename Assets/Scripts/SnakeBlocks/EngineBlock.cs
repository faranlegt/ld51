using Modifiers;
using UnityEngine;

namespace SnakeBlocks
{
    public class EngineBlock : SnakeBlock
    {
        [Space]
        public SpeedModification mod = new();
        
        public override void Modify(Stats stats)
        {
            mod.Apply(stats);
            
            base.Modify(stats);
        }
    }
}