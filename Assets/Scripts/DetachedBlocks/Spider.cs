using System;
using DG.Tweening;
using Effects;
using Emitters.Bullets;
using MyBox;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DetachedBlocks
{
    public class Spider : MonoBehaviour, IDamageListener, IBulletReceiver
    {
        public bool stationary = false;

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

        [Header("sounds")]
        public AudioClip shootClip;

        public AudioClip birthClip;
        
        private AudioSource _audio;

        private void Awake()
        {
            _audio = this.GetOrAddComponent<AudioSource>();
        }

        private void Start()
        {
            if (stationary)
            {
                Singleton<EffectsSpawner>.Instance.Poof(transform.position);
            }
            
            legs.gameObject.SetActive(false);

            body.StartLine(enabling, false);
            body.onFinished.AddListener(Activate);
        }

        private void Activate()
        {
            gameObject.tag = "Obstacle";
            body.onFinished.RemoveAllListeners();

            
            _audio.PlayOneShot(birthClip);
            
            if (!stationary)
            {
                legs.gameObject.SetActive(true);
                legs.StartLine(walking);
            }

            body.StartLine(active);

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
            _audio.PlayOneShot(shootClip);
            Singleton<Bullets>.Instance.Shoot(
                bullet,
                bulletStart.position,
                Random.insideUnitCircle.normalized,
                false
            );
        }

        private void Move()
        {
            if (stationary)
                return;

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

        public void ReceiveDamage()
        {
            Singleton<EffectsSpawner>.Instance.Poof(transform.position);
            Destroy(gameObject);
        }

        public bool Shoot(Bullet b)
        {
            if (!b.isFromPlayer)
                return false;

            ReceiveDamage();

            return true;
        }
    }
}