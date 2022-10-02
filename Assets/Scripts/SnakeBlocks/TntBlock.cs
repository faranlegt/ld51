using Effects;
using MyBox;
using UnityEngine;

namespace SnakeBlocks
{
    public class TntBlock : SnakeBlock
    {
        [Header("Activation")] public SpritesLine explosion;

        public override void Activate()
        {
            _blockAnimator.StartLine(explosion, false);
            _blockAnimator.onFinished.AddListener(ExplodeAround);
        }

        public void ExplodeAround()
        {
            var pos = transform.position;
            Singleton<EffectsSpawner>.Instance.Explode(pos);
            // ReSharper disable once Unity.PreferNonAllocApi
            Collider2D[] found = Physics2D.OverlapBoxAll(pos, Vector2.one * 3, 0);

            foreach (var col in found)
            {
                var go = col.gameObject;
                IExplosionListener[] listeners = go.GetComponents<IExplosionListener>();

                foreach (var explosionListener in listeners)
                {
                    explosionListener.Explode();
                }
            }
        }
    }
}