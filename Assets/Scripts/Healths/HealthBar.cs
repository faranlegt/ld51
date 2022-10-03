using UnityEngine;

namespace Healths
{
    public class HealthBar : MonoBehaviour
    {
        private LineAnimator _lineAnimator;
        private IHealth _health;

        private void Awake()
        {
            _lineAnimator = GetComponent<LineAnimator>();
            _health = GetComponentInParent<IHealth>();
        }

        private void Update()
        {
            _lineAnimator.animationFrame = (int)(_health.Health * (_lineAnimator.sprites.sprites.Length - 2)) + 1;
        }
    }
}