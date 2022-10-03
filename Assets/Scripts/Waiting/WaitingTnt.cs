using Effects;
using Emitters.Bullets;
using MyBox;
using UnityEngine;

namespace Waiting
{
    public class WaitingTnt : MonoBehaviour, IDamageListener, IBulletReceiver
    {
        public void ReceiveDamage()
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
                
                IDamageListener[] listeners = go.GetComponents<IDamageListener>();

                foreach (var explosionListener in listeners)
                {
                    explosionListener.ReceiveDamage();
                }
            }
        }

        public bool Shoot(Bullet b)
        {
            ReceiveDamage();
            return true;
        }
    }
}