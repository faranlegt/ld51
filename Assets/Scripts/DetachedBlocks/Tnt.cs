using Effects;
using MyBox;
using SnakeBlocks;
using UnityEngine;

namespace DetachedBlocks
{
    public class Tnt : MonoBehaviour
    {
        private LineAnimator _animator;

        public void Awake()
        {
            _animator = GetComponent<LineAnimator>();
            _animator.onFinished.AddListener(Explode);
        }

        private void Explode()
        {
            var pos = transform.position;
            Singleton<EffectsSpawner>.Instance.Explode(pos);
            // ReSharper disable once Unity.PreferNonAllocApi
            Collider2D[] found = Physics2D.OverlapBoxAll(pos, Vector2.one * 3, 0);

            foreach (var col in found)
            {
                var go = col.gameObject;
                if (!go) continue;
                
                IExplosionListener[] listeners = go.GetComponents<IExplosionListener>();
                
                foreach (var explosionListener in listeners)
                {
                    explosionListener.Explode();
                }
            }

            Destroy(gameObject);
        }
    }
}