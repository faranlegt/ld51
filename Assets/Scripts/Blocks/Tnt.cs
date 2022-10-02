using System;
using Effects;
using MyBox;
using UnityEngine;

namespace Blocks
{
    public class Tnt : MonoBehaviour
    {
        private LineAnimator _animator;

        public void Awake()
        {
            _animator = GetComponent<LineAnimator>();
            _animator.onFinished.AddListener(Explode);
        }

        private void Explode()
        {
            var pos = transform.position;
            Singleton<EffectsSpawner>.Instance.Explode(pos);
            // ReSharper disable once Unity.PreferNonAllocApi
            Collider2D[] found = Physics2D.OverlapBoxAll(pos, Vector2.one * 3, 0);

            foreach (var col in found)
            {
                var go = col.gameObject;

                if (go.CompareTag("Snake Block"))
                {
                    var block = go.GetComponent<SnakeBlock>();
                    block.snake.RemoveBlock(block);
                }
            }

            Destroy(gameObject);
        }
    }
}