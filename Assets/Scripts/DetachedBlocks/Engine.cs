using Modifiers;
using UnityEngine;

namespace DetachedBlocks
{
    public class Engine : ModifierTile
    {   
        public SpeedModification mod = new();
        
        public SpritesLine detach, tile;
        
        private LineAnimator _animator;

        private void Awake()
        {
            _animator = GetComponent<LineAnimator>();
        }

        private void Start()
        {
            _animator.StartLine(detach, false);
            _animator.onFinished.AddListener(Activate);
        }

        private void Activate()
        {
            _animator.onFinished.RemoveAllListeners();
            _animator.StartLine(tile, true);
        }

        public override Modification GetModification() => mod;
    }
}