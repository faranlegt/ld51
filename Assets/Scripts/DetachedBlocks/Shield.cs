using System;
using Effects;
using MyBox;
using UnityEngine;

namespace DetachedBlocks
{
    public class Shield : MonoBehaviour, IExplosionListener
    {
        public SpritesLine workingLine;

        public ShieldSphere shield;
        
        private LineAnimator _mainRenderer;

        private void Awake()
        {
            _mainRenderer = GetComponent<LineAnimator>();
        }

        private void Start()
        {
            shield.gameObject.SetActive(false);
            
            _mainRenderer.onFinished.AddListener(Activate);
        }

        private void Activate()
        {
            shield.gameObject.SetActive(true);
            
            _mainRenderer.onFinished.RemoveListener(Activate);
            _mainRenderer.StartLine(workingLine, true);
        }

        public void Explode()
        {
            Singleton<EffectsSpawner>.Instance.Explode(transform.position);
            
            Destroy(gameObject);
        }
    }
}