using System;
using System.Security.Cryptography.X509Certificates;
using Cinemachine;
using MyBox;
using SnakeBlocks;
using UnityEngine;

public class SnakeBuilder : MonoBehaviour
{
    public BlockDescription[] startingBlocks;

    public CinemachineVirtualCamera followCamera;
    
    public SnakeBlock emptyBlock;

    private void Start()
    {
        var snakeGo = new GameObject("Snake", typeof(Snake));
        var snake = snakeGo.GetComponent<Snake>();
        snake.followCamera = followCamera;

        for (var i = 0; i < startingBlocks.Length; i++)
        {
            var pos = new Vector3(i, 0, 0);
            snake.Prepend(pos, startingBlocks[i]);
        }

        Singleton<HintsController>.Instance.snake = snake;
        Singleton<HintsController>.Instance.RebuildLists();
    }
}