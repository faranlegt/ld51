using System;
using DG.Tweening;
using Modifiers;
using UnityEngine;

namespace SnakeBlocks
{
    public class SpeedBlock : SnakeBlock
    {   
        [Space]
        public SpeedModification mod = new();

        [Header("Active")]
        public SpeedModification activeModification = new();

        public bool isCharged = true;

        public bool canBeActivated = false;

        public float activeTime = 0.5f;

        public float reloadTime = 2f;

        public bool isActive = false;

        [Header("renderer")]
        public LineAnimator activationRenderer;

        public SpritesLine activation;

        private void Start()
        {
            if (activationRenderer)
            {
                activationRenderer.Hide();
            }
        }

        public override void Modify(Stats stats)
        {
            if (isCharged)
            {
                mod.Apply(stats);
            }
            else if (isActive)
            {
                activeModification.Apply(stats);
            }
            
            base.Modify(stats);
        }

        public override void Update()
        {
            base.Update();

            if (isActive)
            {
                var dir = (endPoint - startingPoint).normalized;
                var effectTransform = activationRenderer.gameObject.transform;
                effectTransform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.down, dir));
            }
        }

        public override void Activate()
        {
            if (!canBeActivated || !isCharged) return;

            isActive = true;
            isCharged = false;
            
            activationRenderer.LaunchOnce(activation);

            DOTween
                .Sequence()
                .PrependInterval(activeTime)
                .OnComplete(
                    () => DOTween
                        .Sequence()
                        .PrependInterval(reloadTime)
                        .OnStart(
                            () =>
                            {
                                isActive = false;
                                activationRenderer.Hide();
                            })
                        .OnComplete(() => isCharged = true)
                );
        }
    }
}