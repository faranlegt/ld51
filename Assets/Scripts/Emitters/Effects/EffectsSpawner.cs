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

        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        public void Explode(Vector3 position)
        {
            CustomEffect(position, explosionEffect).transform.localScale = Vector3.one * 2;
        }

        public void Poof(Vector3 position) => CustomEffect(position, poofEffect);
        
        public void Shoot(Vector3 position) => CustomEffect(position, shootEffect);

        public void Attach(Vector3 position) => CustomEffect(position, poofEffect);

        private Effect CustomEffect(Vector3 position, SpritesLine line)
        {
            var o = Instantiate(effectPrefab, position, Quaternion.identity, _transform);
            o.Init(line);
            return o;
        }
    }
}