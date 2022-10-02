using DG.Tweening;
using Emitters.Bullets;
using MyBox;
using UnityEngine;

namespace SnakeBlocks
{
    public class SpiderBlock : SnakeBlock
    {
        [Header("Spider")]
        public BulletDescription bullet;
        
        [Header("Shooting")] public float reloadTime = 2f;
        public bool canShoot = true;
        public Transform bulletStart;

        [Header("Lines")] public SpritesLine reloading;
        public SpritesLine active;

        public override void Activate()
        {
            if (!canShoot)
                return;

            canShoot = false;

            DOTween
                .Sequence()
                .PrependInterval(reloadTime)
                .OnStepComplete(
                    () =>
                    {
                        canShoot = true;

                        _blockAnimator.StartLine(active);
                    }
                );

            _blockAnimator.StartLine(reloading);

            Shoot();
        }

        private void Shoot()
        {
            var dir = (endPoint - startingPoint).normalized;
            
            Singleton<Bullets>.Instance.Shoot(bullet, bulletStart.position, dir, true);
        }
    }
}