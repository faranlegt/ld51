using System;
using DetachedBlocks;
using DG.Tweening;
using Effects;
using Emitters;
using MyBox;
using UnityEngine;

namespace Pyramid.Phases
{
    public class LastPhase : MonoBehaviour, IPhase
    {
        public WaitingBlock[] enabledDrops = { };

        public int spotsToRemove = 2;
        public Barrel barrel;

        public LineAnimator baseAnimator;

        [Header("Spider spawn")] 
        public Spider spider;

        public Transform spawnPlace;

        public float spawnTime = 7.5f;
        
        private Sequence _spawning;

        private void Start()
        {
            foreach (var waitingBlock in enabledDrops)
            {
                Singleton<WaitingBlockDropper>.Instance.AddBlockForDrop(waitingBlock);
            }

            baseAnimator.onNewFrame.AddListener(CheckFrame);

            _spawning = DOTween
                .Sequence()
                .PrependInterval(spawnTime)
                .OnStepComplete(() => baseAnimator.LaunchOnce(baseAnimator.sprites))
                .SetLoops(-1);
        }

        private void CheckFrame(int frame)
        {
            if (frame != 8)
                return;

            Instantiate(spider, spawnPlace.position, Quaternion.identity);
        }

        public void WeakSpotRemoved()
        {
            spotsToRemove--;

            if (spotsToRemove <= 0)
            {
                FinishGame();
            }
        }

        private void FinishGame()
        {
            _spawning.Complete();
            Singleton<EffectsSpawner>.Instance.Explode(transform.position, 3);
            barrel.Destroy();
        }
    }
}