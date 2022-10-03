using System;
using Effects;
using Emitters.Bullets;
using Healths;
using MyBox;
using Pyramid.Phases;
using UnityEngine;

namespace Pyramid
{
    public class WeekSpot : MonoBehaviour, IDamageListener, IBulletReceiver, IHealth
    {
        public int maxHealth = 3;

        public int health;

        public GameObject damageObject;

        public Transform explosionPoint;

        private IPhase _phase;

        private void Awake()
        {
            _phase = GetComponentInParent<IPhase>();
            explosionPoint = transform.Find("Explosion Point");
        }

        private void Start()
        {
            if (damageObject)
            {
                damageObject.SetActive(false);
            }

            health = maxHealth;
        }

        public void ReceiveDamage()
        {
            Damage(1);
        }

        public bool Shoot(Bullet b)
        {
            if (!b.isFromPlayer)
                return false;

            Damage(1);
            return true;
        }

        public void Damage(int h)
        {
            Singleton<EffectsSpawner>.Instance.Explode(explosionPoint.position, 1);
            health -= h;

            if (health <= 0)
            {
                RemoveSelf();
            }
        }

        private void RemoveSelf()
        {
            if (damageObject)
            {
                damageObject.SetActive(true);
            }

            Destroy(gameObject);

            _phase.WeakSpotRemoved();
        }

        public float Health => health / (float)maxHealth;
    }
}