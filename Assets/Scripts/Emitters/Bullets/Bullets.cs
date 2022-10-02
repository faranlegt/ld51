using Effects;
using MyBox;
using UnityEngine;

namespace Emitters.Bullets
{
    public class Bullets : MonoBehaviour
    {
        public Bullet bullet;
        
        public void Shoot(BulletDescription desc, Vector3 start, Vector2 dir, bool fromPlayer)
        {
            Instantiate(bullet, start, Quaternion.identity).Init(desc, fromPlayer, dir);

            Singleton<EffectsSpawner>.Instance.Shoot(start);
        }
    }
}