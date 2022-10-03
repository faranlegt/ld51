using System.Collections.Generic;
using Cinemachine;
using Effects;
using Emitters;
using Modifiers;
using MyBox;
using SnakeBlocks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Snake : MonoBehaviour
{
    public float baseSpeed = 4f;

    public Stats stats = new();

    public Vector2 direction = Vector2.right;

    public CinemachineVirtualCamera followCamera;

    public bool requestDeath = false;

    [ReadOnly] public SnakeBlock tail;
    [ReadOnly] public SnakeBlock head;

    [ReadOnly] public int blocksIndex = 0;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        if (requestDeath)
        {
            CheckDeath();
        }

        stats.speed = baseSpeed;
        head.Modify(stats);

        if (Keyboard.current.upArrowKey.isPressed && direction != Vector2.down)
        {
            direction = Vector2.up;
        }
        if (Keyboard.current.leftArrowKey.isPressed && direction != Vector2.right)
        {
            direction = Vector2.left;
        }
        if (Keyboard.current.downArrowKey.isPressed && direction != Vector2.up)
        {
            direction = Vector2.down;
        }
        if (Keyboard.current.rightArrowKey.isPressed && direction != Vector2.left)
        {
            direction = Vector2.right;
        }
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            head.Activate();
        }
    }

    private void CheckDeath()
    {
        var n = head;
        List<SnakeBlock> blocks = new();

        while (n)
        {
            if (!n.isStopped)
                return;

            blocks.Add(n);

            n = n.child;
        }

        foreach (var b in blocks)
        {
            Singleton<EffectsSpawner>.Instance.Poof(b.transform.position);
        }

        Lose();
        Destroy(gameObject);
    }

    public SnakeBlock Prepend(Vector3 position, BlockDescription blockDescription)
    {
        var newBlock = Instantiate(
            blockDescription.baseBlock,
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
        Singleton<HintsController>.Instance.RebuildLists();

        return newBlock;
    }

    public void Stop() => head.Stop();

    public void Restart() => head.Restart();

    public void LoseTail()
    {
        if (tail == head)
        {
            Lose();
            return;
        }

        tail.Detach();

        tail = tail.parent;
        
        Singleton<HintsController>.Instance.RebuildLists();
    }

    public void RemoveBlock(SnakeBlock block)
    {
        if (block == head)
        {
            head = block.child;
            followCamera.Follow = head.transform;

            if (block == tail)
            {
                Lose();
            }
        }
        else if (block == tail)
        {
            tail = block.parent;
        }
        else
        {
            var p = block.parent;
            var c = block.child;

            p.child = c;
            c.parent = p;

            c.StartMoving(c.transform.position, c.moveDuration);
        }

        Destroy(block.gameObject);
        Singleton<HintsController>.Instance.RebuildLists();
    }

    private void Lose()
    {
        FindObjectOfType<DeathCanvas>(true).gameObject.SetActive(true);
    }

    public void Die()
    {
        requestDeath = true;
        head.Stop();
    }

    public Vector3 GetNextPosition() => (head.transform.position + (Vector3)direction).SnapToOne();
}