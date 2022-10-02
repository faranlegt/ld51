using System;
using Emitters.Bullets;
using UnityEngine;

public class ShieldSphere : MonoBehaviour, IBulletReceiver
{
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    public void Init(float size)
    {
        _transform.localScale = size * Vector3.one;
    }

    public bool Shoot(Bullet b) => true;
}