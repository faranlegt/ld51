using System;
using MyBox;
using UnityEngine;

namespace Pyramid
{
    public class BarrelBullet : MonoBehaviour
    {
        public float speed;

        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        private void Update()
        {
            var snake = Singleton<Snake>.Instance;
            
            if (!snake) return;
            
            var tail = snake.tail;

            if (!tail)
            {
                Destroy(gameObject);
            }

            var target = tail.transform.position;
            var position = _transform.position;
            var pos = position;
            var delta = target - pos;

            if (delta.magnitude < speed * Time.deltaTime)
            {
                Destroy(gameObject);
                snake.LoseTail();
            }
            else
            {
                _transform.position = pos + speed * Time.deltaTime * delta.normalized;
            }
        }
    }
}