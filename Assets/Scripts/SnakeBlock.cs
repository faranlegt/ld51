using System;
using MyBox;
using Unity.VisualScripting;
using UnityEngine;

public class SnakeBlock : MonoBehaviour
{
    public BlockDescription description;

    [Space, Header("Movement"), ReadOnly] public Vector3 startingPoint;

    [ReadOnly] public Vector3 endPoint;

    [ReadOnly] public float moveDuration;

    [Space, Header("Renderers"), MustBeAssigned]
    public LineAnimator _blockAnimator;

    [MustBeAssigned] public LineAnimator _tracksAnimator;

    private Snake _snake;
    private SnakeBlock _parent, _child;
    private Transform _transform;


    private void Awake()
    {
        _snake = GetComponentInParent<Snake>();
        _transform = transform;
    }

    public void Init(SnakeBlock child, BlockDescription blockDescription)
    {
        _child = child;
        description = blockDescription;

        _blockAnimator.StartLine(description.block, true);
        _tracksAnimator.StartLine(description.trackHorizontal, true);
    }

    public void SetParent(SnakeBlock parent)
    {
        _parent = parent;
    }

    public void StartMoving(Vector3 from, float durationOffset = 0f)
    {
        var nextPosition = IsHead
            ? _snake.GetNextPosition()
            : _parent.transform.position;

        startingPoint = from;
        endPoint = nextPosition;

        var delta = (endPoint - startingPoint);
        var tracksLine = (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            ? description.trackHorizontal
            : description.trackVertical;

        _tracksAnimator.StartLine(tracksLine, true);

        moveDuration = durationOffset;
    }

    private void Update()
    {
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

    public bool IsHead => !_parent;
}