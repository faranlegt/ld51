using DG.Tweening;
using MyBox;
using UnityEngine;

namespace Pyramid
{
    public class Barrel : MonoBehaviour
    {
        public BarrelBullet bulletPrefab;

        public Transform bulletSpawn;

        public SpritesLine shootAnimation;

        private LineAnimator _animator;

        private void Awake()
        {
            _animator = GetComponent<LineAnimator>();
            _animator.onNewFrame.AddListener(CheckFrame);

            DOTween
                .Sequence()
                .PrependInterval(10)
                .OnStepComplete(() => _animator.LaunchOnce(shootAnimation))
                .SetLoops(-1);
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