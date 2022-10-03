using UnityEngine;

namespace Pyramid.Phases
{
    public class SecondPhase : MonoBehaviour, IPhase
    {
        public GameObject nextPhasePrefab;
        
        public int spotsToRemove = 2;
        
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
            Destroy(gameObject);
            
            Instantiate(nextPhasePrefab, transform.position, Quaternion.identity);
        }
    }
}