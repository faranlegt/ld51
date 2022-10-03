using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class LineAnimator : MonoBehaviour
{
    public SpritesLine sprites;
    public int animationFrame = 0;
    public bool loop;
    public bool reverse;
    public bool animate;
    public float frameLength = 1f;

    public UnityEvent onFinished;
    public UnityEvent<int> onNewFrame;

    private SpriteRenderer _renderer;
    private SpritesLine _oldSprites;

    private bool _revalidate;
    private float _frameTime;
    private int _oldFrame;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        
        onFinished ??= new UnityEvent();
        onNewFrame ??= new UnityEvent<int>();

        SyncSprite();
    }

    private void Start()
    {
        _revalidate = true;
    }

    public void StartLine(SpritesLine line, bool? loop = null, bool? reverse = null)
    {
        sprites = line;
        animationFrame = 0;
        animate = true;
        this.loop = loop ?? this.loop;
        this.reverse = reverse ?? this.reverse;
    }

    public void LaunchOnce(SpritesLine line)
    {
        StartLine(line, loop: false);
    }

    private void OnValidate()
    {
        _revalidate = true;
    }

    private void LateUpdate()
    {
        _revalidate |= _oldFrame != animationFrame || _oldSprites != sprites;

        if (_revalidate)
        {
            SyncSprite();
        }

        if (animate)
        {
            _frameTime += Time.deltaTime;
            if (_frameTime >= frameLength)
            {
                NextFrame();
            }
        }

        _oldFrame = animationFrame;
        _oldSprites = sprites;
    }

    private void NextFrame()
    {
        _frameTime = 0;
        if (reverse)
        {
            if (animationFrame <= 0)
            {
                if (loop)
                {
                    animationFrame = sprites.sprites.Length - 1;
                    onNewFrame.Invoke(animationFrame);
                }
                else
                {
                    animate = false;
                    onFinished?.Invoke();
                }
            }
            else
            {
                animationFrame--;
                onNewFrame.Invoke(animationFrame);
            }
        }
        else
        {
            if (animationFrame >= sprites.sprites.Length - 1)
            {
                if (loop)
                {
                    animationFrame = 0;
                    onNewFrame.Invoke(animationFrame);
                }
                else
                {
                    animate = false;
                    onFinished?.Invoke();
                }
            }
            else
            {
                animationFrame++;
                onNewFrame.Invoke(animationFrame);
            }
        }

        SyncSprite();
    }

    private void SyncSprite()
    {
        _renderer.sprite = sprites.sprites[animationFrame];
        _revalidate = false;
    }

    //private void OnDestroy() => _animationEnd?.Dispose();
    public void Hide()
    {
        animate = false;
        _renderer.sprite = null;
    }
}