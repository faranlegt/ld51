using Effects;
using Emitters;
using MyBox;
using UnityEngine;

namespace Pyramid.Phases
{
    public class LastPhase : MonoBehaviour, IPhase
    {
        public WaitingBlock[] enabledDrops = {};
        
        public int spotsToRemove = 2;
        public Barrel barrel;

        private void Start()
        {
            foreach (var waitingBlock in enabledDrops)
            {
                Singleton<WaitingBlockDropper>.Instance.AddBlockForDrop(waitingBlock);
            }
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
            Singleton<EffectsSpawner>.Instance.Explode(transform.position, 3);
            barrel.Destroy();
        }
    }
}