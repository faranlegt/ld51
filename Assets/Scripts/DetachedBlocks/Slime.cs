using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DetachedBlocks
{
    public class Slime : MonoBehaviour, IExplosionListener
    {
        public Slime slime;

        public int movesToRespawn = 10;
        public float moveTimer = 3f;
        public Ease moveEase = Ease.OutCubic;
        public float moveDuration = 1f;

        public int movesCount;

        private void Start() =>
            DOTween
                .Sequence()
                .PrependInterval(moveTimer)
                .SetLoops(-1)
                .OnStepComplete(Move);

        private void Move()
        {
            movesCount++;
            var pos = transform.position;

            if (movesCount % movesToRespawn == 0)
            {
                Instantiate(slime, pos, Quaternion.identity);
            }
            
            var angleOffset = Mathf.PI / 2 * Random.Range(0, 4);

            for (var i = 0; i < 4; i++)
            {
                angleOffset += Mathf.PI / 2;
                var direction = new Vector3(Mathf.Sin(angleOffset), Mathf.Cos(angleOffset));

                var c = Physics2D.OverlapBox(pos + direction, Vector2.one, 0);

                if (c) continue;

                transform.DOMove(pos + direction, moveDuration).SetEase(moveEase);
                return;
            }
        }

        public void Explode()
        {
            Destroy(gameObject);
        }
    }
}