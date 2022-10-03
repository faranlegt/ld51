using DG.Tweening;
using Effects;
using MyBox;
using UnityEngine;

namespace Pyramid
{
    public class Barrel : MonoBehaviour
    {
        public BarrelBullet bulletPrefab;

        public Transform bulletSpawn;

        public SpritesLine shootAnimation, destroy;

        private LineAnimator _animator;
        private Sequence _shooting;

        private void Awake()
        {
            _animator = GetComponent<LineAnimator>();
            _animator.onNewFrame.AddListener(CheckFrame);

            _shooting = DOTween
                .Sequence()
                .PrependInterval(10)
                .OnComplete(
                    () =>
                    {
                        _animator.LaunchOnce(destroy);
                        Singleton<EffectsSpawner>
                            .Instance
                            .Explode(transform.position, 3);
                    })
                .SetLoops(-1);
            
            _shooting.ManualUpdate(Time.time, 0);
            _shooting.OnStepComplete(() => _animator.LaunchOnce(shootAnimation));
        }

        public void Destroy()
        {
            _shooting.Complete();
        }

        private void CheckFrame(int frame)
        {
            if (frame != 3)
                return; // Aaa

            Shoot();
        }

        private void Shoot() => Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
    }
}