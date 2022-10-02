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

        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        public void Explode(Vector3 position) => CustomEffect(position, explosionEffect);

        public void Poof(Vector3 position) => CustomEffect(position, poofEffect);

        public void Attach(Vector3 position) => CustomEffect(position, poofEffect);

        private void CustomEffect(Vector3 position, SpritesLine line)
        {
            Instantiate(effectPrefab, position, Quaternion.identity, _transform).Init(line);
        }
    }
}