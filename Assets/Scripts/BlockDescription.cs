using SnakeBlocks;
using UnityEngine;

[CreateAssetMenu(fileName = "Block", menuName = "Snakur/Block")]
public class BlockDescription : ScriptableObject
{
    public SpritesLine block;

    public Sprite blockWaiting;

    [Space, Header("Tracks")] public SpritesLine trackHorizontal;

    public SpritesLine trackVertical;

    [Header("Prefabs")] public SnakeBlock baseBlock;

    [Header("Detach")] public bool poofOnDetach = true;

    public GameObject detachedBlock;

    [Header("UI")] public Sprite headUiImage;
    public Sprite activeUiImage;
    public Sprite passiveImage;
    public Sprite lastUiImage;
}