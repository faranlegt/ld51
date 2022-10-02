using System;
using DG.Tweening;
using Emitters.Bullets;
using MyBox;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DetachedBlocks
{
    public class Spider : MonoBehaviour, IExplosionListener, IBulletReceiver
    {
        [Header("Lines")] public SpritesLine enabling;
        public SpritesLine walking;
        public SpritesLine active;

        [Space, Header("Animators")] public LineAnimator body;
        public LineAnimator legs;

        [Space, Header("Movement")] public float moveDuration = 1f;
        public Ease moveEase = Ease.Linear;

        [Space, Header("Shoot")] public float shootDelay = 3f;
        public BulletDescription bullet;

        public Transform bulletStart;

        private void Start()
        {
            legs.gameObject.SetActive(false);


            body.StartLine(enabling, false);
            body.onFinished.AddListener(Activate);
        }

        private void Activate()
        {
            gameObject.tag = "Obstacle";
            body.onFinished.RemoveAllListeners();

            legs.gameObject.SetActive(true);

            body.StartLine(active);
            legs.StartLine(walking);

            DOTween
                .Sequence()
                .PrependInterval(moveDuration)
                .SetLoops(-1)
                .OnStepComplete(Move);

            DOTween
                .Sequence()
                .PrependInterval(shootDelay)
                .SetLoops(-1)
                .OnStepComplete(Shoot);
        }

        private void Shoot()
        {
            Singleton<Bullets>.Instance.Shoot(
                bullet,
                bulletStart.position,
                Random.insideUnitCircle.normalized,
                true
            );
        }

        private void Move()
        {
            var t = transform;
            var pos = t.position;

            var angleOffset = Mathf.PI / 2 * Random.Range(0, 4);

            for (var i = 0; i < 4; i++)
            {
                angleOffset += Mathf.PI / 2;
                var direction = new Vector3(Mathf.Sin(angleOffset), Mathf.Cos(angleOffset));

                var c = Physics2D.OverlapBox(pos + direction, Vector2.one, 0);

                if (c)
                    continue;

                t.DOMove(pos + direction, moveDuration).SetEase(moveEase);
                return;
            }
        }

        public void Explode()
        {
            Destroy(gameObject);
        }

        public bool Shoot(Bullet b)
        {
            if (!b.isFromPlayer)
                return false;

            Destroy(gameObject);

            return true;
        }
    }
}