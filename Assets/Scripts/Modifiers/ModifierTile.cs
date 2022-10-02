using UnityEngine;

namespace Modifiers
{
    public abstract class ModifierTile : MonoBehaviour
    {
        public abstract void Apply(Stats stats);
    }
}