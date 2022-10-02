using Effects;
using Emitters.Bullets;
using MyBox;
using UnityEngine;

namespace Waiting
{
    public class WaitingTnt : MonoBehaviour, IExplosionListener, IBulletReceiver
    {
        public void Explode()
        {
            Destroy(gameObject);
            
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
        }

        public bool Shoot(Bullet b)
        {
            Explode();
            return true;
        }
    }
}