using System;
using DG.Tweening;
using Effects;
using MyBox;
using UnityEngine;

public class SnakeBlock : MonoBehaviour
{
    public BlockDescription description;

    [Space, Header("Movement"), ReadOnly] public Vector3 startingPoint;

    [ReadOnly] public Vector3 endPoint;

    [ReadOnly] public float moveDuration;

    public bool isStopped;

    [Space, Header("Renderers"), MustBeAssigned]
    public LineAnimator _blockAnimator;

    [MustBeAssigned] public LineAnimator _tracksAnimator;

    [Header("Connections"), ReadOnly] public SnakeBlock parent;

    [ReadOnly] public SnakeBlock child;

    private Snake _snake;
    private Transform _transform;

    private Tween _movementTween;


    private void Awake()
    {
        _snake = GetComponentInParent<Snake>();
        _transform = transform;
    }

    private void Update()
    {
        if (isStopped)
        {
            return;
        }

        moveDuration += Time.deltaTime / _snake.GetBaseMovementDuration();

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

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Snake Block"))
        {
            var block = col.gameObject.GetComponent<SnakeBlock>();

            if (block.child == this && block.isStopped)
            {
                Stop();
            }
        }
        else if (col.gameObject.CompareTag("Waiting Block"))
        {
            if (!IsHead) return;
            
            var waitingBlock = col.gameObject.GetComponent<WaitingBlock>();
            var position = waitingBlock.transform.position;
            var newBlock = _snake.Prepend(position, waitingBlock.description);

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

        isStopped = false;

        _movementTween.Play();

        if (child)
        {
            child.Restart();
        }
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

    public void StartMoving(Vector3 from, float durationOffset = 0f)
    {
        var nextPosition = IsHead
            ? _snake.GetNextPosition()
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