using Effects;
using MyBox;
using UnityEngine;

namespace Pyramid.Phases
{
    public class FirstPhase : MonoBehaviour, IPhase
    {
        public GameObject nextPhasePrefab;
        
        public int spotsToRemove = 4;
        
        public void WeakSpotRemoved()
        {
            spotsToRemove--;

            if (spotsToRemove <= 0)
            {
                NextPhase();
            }
        }

        private void NextPhase()
        {
            Singleton<EffectsSpawner>.Instance.Explode(transform.position, 3);
            
            Destroy(gameObject);
            
            Instantiate(nextPhasePrefab, transform.position, Quaternion.identity);
        }
    }
}