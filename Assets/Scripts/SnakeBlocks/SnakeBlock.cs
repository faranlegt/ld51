using System;
using System.Collections.Generic;
using DG.Tweening;
using Effects;
using Emitters.Bullets;
using Modifiers;
using MyBox;
using UnityEngine;

namespace SnakeBlocks
{
    public class SnakeBlock : MonoBehaviour, IDamageListener, IBulletReceiver
    {
        public BlockDescription description;

        public List<Modification> modifications = new();

        [Space, Header("Movement"), ReadOnly] public Vector3 startingPoint;

        [ReadOnly] public Vector3 endPoint;

        [ReadOnly] public float moveDuration;

        public bool isDamaging;

        public bool isStopped;

        [Space, Header("Renderers"), MustBeAssigned]
        public LineAnimator _blockAnimator;

        [MustBeAssigned] public LineAnimator _tracksAnimator;

        [Header("Connections"), ReadOnly] public SnakeBlock parent;

        [ReadOnly] public SnakeBlock child;

        [ReadOnly] public Snake snake;

        private Transform _transform;

        private Tween _movementTween;


        public void Awake()
        {
            snake = GetComponentInParent<Snake>();
            _transform = transform;
        }

        public virtual void Update()
        {
            if (isStopped)
            {
                return;
            }

            var s = snake.stats.Copy();
            foreach (var modification in modifications)
            {
                modification.Apply(s);
            }

            moveDuration += Time.deltaTime * s.speed;

            var nextPos = Vector3.Lerp(startingPoint, endPoint, moveDuration);

            if (moveDuration > 1)
            {
                transform.position = endPoint;

                StartMoving(endPoint, moveDuration - 1);
            }
            else
            {
                transform.position = nextPos;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (isDamaging && collision.gameObject.HasComponent<IDamageListener>())
            {
                if (collision.gameObject.HasComponent<SnakeBlock>()) return;
                
                var d = collision.gameObject.GetComponent<IDamageListener>();
                d.ReceiveDamage();
            }
            
            if (collision.gameObject.CompareTag("Modifier"))
            {
                var m = collision.gameObject.GetComponent<ModifierTile>();
                modifications.Add(m.GetModification());
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Modifier"))
            {
                var m = collision.gameObject.GetComponent<ModifierTile>();
                modifications.Remove(m.GetModification());
            }
        }

        private void OnCollisionStay2D(Collision2D col)
        {
            if (isDamaging && col.gameObject.HasComponent<IDamageListener>())
            {
                return;
            }
            
            if (col.gameObject.CompareTag("Snake Block"))
            {
                var block = col.gameObject.GetComponent<SnakeBlock>();

                if (block.child == this && block.isStopped)
                {
                    Stop();
                }
            }
            else if (col.gameObject.CompareTag("Obstacle"))
            {
                if (!IsHead)
                    return;

                snake.Die();
            }
            else if (col.gameObject.CompareTag("Waiting Block"))
            {
                if (!IsHead)
                    return;

                var waitingBlock = col.gameObject.GetComponent<WaitingBlock>();
                var position = waitingBlock.transform.position;
                var newBlock = snake.Prepend(position, waitingBlock.description);

                DOTween
                    .Sequence()
                    .PrependInterval(0.5f)
                    .OnStart(newBlock.Stop)
                    .OnComplete(newBlock.Restart);

                Singleton<EffectsSpawner>.Instance.Attach(position);

                Destroy(waitingBlock.gameObject);
            }
        }

        public void Init(SnakeBlock newChild, BlockDescription blockDescription, int blocksIndex)
        {
            child = newChild;
            description = blockDescription;

            _blockAnimator.StartLine(description.block, true);
            _tracksAnimator.StartLine(description.trackHorizontal, true);

            _movementTween = _blockAnimator.transform
                .DOLocalMoveY(0.1f, 0.2f)
                .SetRelative(true)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);

            _movementTween.OnPause(
                () => DOTween
                    .Sequence()
                    .Append(
                        _blockAnimator
                            .transform
                            .DOLocalMove((endPoint - startingPoint).normalized * 0.2f, 0.2f)
                            .SetRelative(true)
                            .SetEase(Ease.OutQuart)
                    ).Append(
                        _blockAnimator
                            .transform
                            .DOLocalMove(-(endPoint - startingPoint).normalized * 0.2f, 0.2f)
                            .SetRelative(true)
                            .SetEase(Ease.Linear)
                    )
            );

            _movementTween.ManualUpdate(Time.time + 0.2f * (blocksIndex % 2), 0);
        }

        public void Stop()
        {
            isStopped = true;
            _movementTween.Pause();
        }

        public void Restart()
        {
            if (!isStopped)
                return;

            if (moveDuration == 0)
            {
                StartMoving(transform.position);
            }

            isStopped = false;

            _movementTween.Play();

            if (child)
            {
                child.Restart();
            }
        }

        public void ReceiveDamage()
        {
            snake.RemoveBlock(this);
        }

        public bool Shoot(Bullet b)
        {
            if (b.isFromPlayer)
                return false;

            snake.RemoveBlock(this);
            
            return true;
        }

        public void Detach()
        {
            if (description.poofOnDetach)
            {
                Singleton<EffectsSpawner>.Instance.Poof(transform.position);
            }

            if (description.detachedBlock)
            {
                Instantiate(
                    description.detachedBlock,
                    _transform.position.SnapToOne(),
                    Quaternion.identity
                );
            }

            Destroy(gameObject);
        }

        public void SetParent(SnakeBlock p)
        {
            parent = p;
        }

        public virtual void Modify(Stats stats)
        {
            if (child)
            {
                child.Modify(stats);
            }
        }

        public virtual void Activate() { }

        public void StartMoving(Vector3 from, float durationOffset = 0f)
        {
            var nextPosition = IsHead
                ? snake.GetNextPosition()
                : parent.transform.position;

            startingPoint = from;
            endPoint = nextPosition;

            var delta = (endPoint - startingPoint);
            var horizontal = Mathf.Abs(delta.x) > Mathf.Abs(delta.y);
            var tracksLine = horizontal
                ? description.trackHorizontal
                : description.trackVertical;
            var reverse = horizontal
                ? delta.x < 0
                : delta.y < 0;

            _tracksAnimator.StartLine(tracksLine, true, reverse);

            moveDuration = durationOffset;
        }

        public bool IsHead => !parent;
    }
}