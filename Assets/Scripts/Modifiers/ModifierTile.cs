using UnityEngine;

namespace Modifiers
{
    public abstract class ModifierTile : MonoBehaviour
    {
        public abstract Modification GetModification();
    }
}