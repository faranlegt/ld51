using System;
using System.Collections.Generic;
using Effects;
using MyBox;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Emitters
{
    public class WaitingBlockDropper : MonoBehaviour
    {
        public float timeBetweenSpawns = 5f;

        public float timer = 0f;

        public float radiusOfDrop = 15f;
        
        public List<WaitingBlock> blocksToDrop = new();

        private void Update()
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                Drop();
                timer = timeBetweenSpawns;
            }
        }

        private void Drop()
        {
            var found = false;
            var pos = Vector2.zero;

            while (!found)
            {
                pos = (Random.insideUnitCircle * radiusOfDrop).SnapToOne();

                if (!Physics2D.OverlapBox(pos, Vector2.one, 0))
                {
                    found = true;
                }
            }

            Instantiate(blocksToDrop.GetRandom(), pos, quaternion.identity);
            Singleton<EffectsSpawner>.Instance.Poof(pos);
        }

        public void AddBlockForDrop(WaitingBlock block)
        {
            blocksToDrop.Add(block);
        }
    }
}