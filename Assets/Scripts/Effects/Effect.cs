using System;
using UnityEngine;

namespace Effects
{
    public class Effect : MonoBehaviour
    {
        private LineAnimator _animator;

        private void Awake()
        {
            _animator = GetComponent<LineAnimator>();
        }

        public void Init(SpritesLine line)
        {
            _animator.finished.AddListener(Die);
            _animator.StartLine(line, false);
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}