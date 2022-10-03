using System;
using UnityEngine;

namespace SnakeBlocks
{
    public class ShieldBlock : SnakeBlock
    {
        [Header("Shields")] public ShieldSphere shieldPrefab;

        public ShieldSphere ownShield, childShield, parentShield;

        [Header("Animations")] public LineAnimator headAnimator;

        public LineAnimator glassAnimator;

        [Header("State")] public bool isActive;

        [Header("Sounds")] public AudioClip activate;
        public AudioClip deactivate;

        private void Start()
        {
            headAnimator.onFinished.AddListener(ActivateShields);
            glassAnimator.gameObject.SetActive(false);
            
            AudioSource.PlayOneShot(activate);
        }

        private void ActivateShields()
        {
            isActive = true;
            glassAnimator.gameObject.SetActive(true);
        }

        public override void Update()
        {
            if (isActive)
            {
                UpdateShield(ref ownShield, this);
                UpdateShield(ref childShield, child);
                UpdateShield(ref parentShield, parent);
            }

            base.Update();
        }

        private void UpdateShield(ref ShieldSphere shield, SnakeBlock block)
        {
            if (shield)
            {
                if (block)
                {
                    shield.transform.position = block.transform.position;
                }
                else
                {
                    Destroy(shield.gameObject);
                }
            }
            else if (block)
            {
                shield = Instantiate(
                    shieldPrefab,
                    block.transform.position,
                    Quaternion.identity
                );
                shield.Init(1);
            }
        }

        private void OnDestroy()
        {
            Destroy(ownShield.gameObject);

            if (childShield && childShield.gameObject)
            {
                Destroy(childShield.gameObject);
            }

            if (parentShield && parentShield.gameObject)
            {
                Destroy(parentShield.gameObject);
            }
        }
    }
}