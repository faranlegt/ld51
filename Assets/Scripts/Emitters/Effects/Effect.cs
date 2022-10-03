using System;
using MyBox;
using UnityEngine;

namespace Effects
{
    public class Effect : MonoBehaviour
    {
        private LineAnimator _animator;
        private AudioSource _audioSource;

        private void Awake()
        {
            _animator = GetComponent<LineAnimator>();
            _audioSource = this.GetOrAddComponent<AudioSource>();
        }

        public void Init(SpritesLine line, AudioClip clip)
        {
            _animator.onFinished.AddListener(Die);
            _animator.StartLine(line, false);

            if (clip)
            {
                _audioSource.PlayOneShot(clip);
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}