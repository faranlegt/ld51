using System;
using Cinemachine;
using DG.Tweening;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Snake : MonoBehaviour
{
    public SnakeBlock blockTemplate;

    public float baseSpeed = 4f;

    public Vector2 direction = Vector2.right;
    
    public CinemachineVirtualCamera followCamera;

    [ReadOnly] public SnakeBlock tail;
    [ReadOnly] public SnakeBlock head;

    [ReadOnly] public int blocksIndex = 0;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void Start()
    {
        // DOTween
        //     .Sequence()
        //     .PrependInterval(10)
        //     .SetLoops(-1)
        //     .OnComplete(LoseTail);
    }

    private void Update()
    {
        if (Keyboard.current.wKey.isPressed)
        {
            direction = Vector2.up;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            direction = Vector2.left;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            direction = Vector2.down;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            direction = Vector2.right;
        }
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            LoseTail();
        }
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            Stop();
        }
        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            Restart();
        }
    }

    public SnakeBlock Prepend(Vector3 position, BlockDescription blockDescription)
    {
        var newBlock = Instantiate(
            blockTemplate,
            position,
            Quaternion.identity,
            _transform
        );

        newBlock.Init(head, blockDescription, blocksIndex++);

        if (head)
        {
            head.SetParent(newBlock);
        }
        
        head = newBlock;

        if (!tail)
        {
            tail = newBlock;
        }

        newBlock.StartMoving(position);

        followCamera.Follow = newBlock.transform;

        return newBlock;
    }

    public void Stop() => head.Stop();

    public void Restart() => head.Restart();

    public void LoseTail()
    {
        if (tail == head)
        {
            Debug.LogError("Oooops, lose");
            return;
        }

        tail.Detach();

        tail = tail.parent;
    }

    public float GetBaseMovementDuration() => 1 / baseSpeed;

    public Vector3 GetNextPosition() => head.transform.position + (Vector3)direction;
}