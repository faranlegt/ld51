using System;
using UnityEngine;

namespace Emitters.Bullets
{
    public class Bullet : MonoBehaviour
    {
        public BulletDescription description;

        public Vector2 direction;
        public bool isFromPlayer;
        
        private SpriteRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        public void Init(BulletDescription desc, bool fromPlayer, Vector2 dir)
        {
            isFromPlayer = fromPlayer;
            description = desc;
            direction = dir.normalized;

            _renderer.sprite = desc.sprite;
        }

        private void Update()
        {
            transform.position += (Vector3)direction * description.speed * Time.deltaTime;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            IBulletReceiver[] receivers = col.gameObject.GetComponents<IBulletReceiver>();

            foreach (var r in receivers)
            {
                if (r.Shoot(this))
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}