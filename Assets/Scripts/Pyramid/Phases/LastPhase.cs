using Effects;
using MyBox;
using UnityEngine;

namespace Pyramid.Phases
{
    public class LastPhase : MonoBehaviour, IPhase
    {
        public int spotsToRemove = 2;
        public Barrel barrel;
        
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