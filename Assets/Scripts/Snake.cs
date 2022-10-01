using System;
using Cinemachine;
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

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
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
    }

    public SnakeBlock Prepend(Vector3 position, BlockDescription blockDescription)
    {
        var newBlock = Instantiate(
            blockTemplate,
            position,
            Quaternion.identity,
            _transform
        );

        newBlock.Init(head, blockDescription);

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

    public float GetBaseMovementDuration() => 1 / baseSpeed;

    public Vector3 GetNextPosition() => head.transform.position + (Vector3)direction;
}