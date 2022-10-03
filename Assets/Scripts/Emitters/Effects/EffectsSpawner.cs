using System;
using UnityEngine;

namespace Effects
{
    public class EffectsSpawner : MonoBehaviour
    {
        public Effect effectPrefab;

        [Space] public SpritesLine explosionEffect;
        public SpritesLine poofEffect;
        public SpritesLine attachEffect;
        public SpritesLine shootEffect;

        [Header("sounds")] public AudioClip poofSound;
        
        public AudioClip explosionSound;

        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        public void Explode(Vector3 position, float size = 2)
        {
            CustomEffect(position, explosionEffect, explosionSound).transform.localScale = Vector3.one * size;
        }

        public void Poof(Vector3 position) => CustomEffect(position, poofEffect, poofSound);
        
        public void Shoot(Vector3 position) => CustomEffect(position, shootEffect, null);

        public void Attach(Vector3 position) => CustomEffect(position, poofEffect, poofSound);

        private Effect CustomEffect(Vector3 position, SpritesLine line, AudioClip clip)
        {
            var o = Instantiate(effectPrefab, position, Quaternion.identity, _transform);
            o.Init(line, clip);
            return o;
        }
    }
}