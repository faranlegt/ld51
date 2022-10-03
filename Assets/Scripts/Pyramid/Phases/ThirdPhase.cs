using System;
using Effects;
using Emitters;
using MyBox;
using UnityEngine;

namespace Pyramid.Phases
{
    public class ThirdPhase : MonoBehaviour, IPhase
    {
        public WaitingBlock[] enabledDrops = {};
        
        public GameObject nextPhasePrefab;
        
        public int spotsToRemove = 2;

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