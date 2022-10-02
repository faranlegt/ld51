using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DetachedBlocks
{
    public class Spider : MonoBehaviour, IExplosionListener
    {
        [Header("Lines")] public SpritesLine enabling;
        public SpritesLine walking;
        public SpritesLine active;

        [Space, Header("Animators")] public LineAnimator body;
        public LineAnimator legs;

        [Space, Header("Movement")] public float moveDuration = 1f;
        public Ease moveEase = Ease.Linear;

        private void Start()
        {
            legs.gameObject.SetActive(false);


            body.StartLine(enabling, false);
            body.onFinished.AddListener(Activate);
        }

        private void Activate()
        {
            body.onFinished.RemoveAllListeners();

            legs.gameObject.SetActive(true);

            body.StartLine(active);
            legs.StartLine(walking);

            DOTween
                .Sequence()
                .PrependInterval(moveDuration)
                .SetLoops(-1)
                .OnStepComplete(Move);
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
    }
}