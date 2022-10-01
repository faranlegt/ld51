using UnityEngine;

[CreateAssetMenu(fileName = "Block", menuName = "Snakur/Block")]
public class BlockDescription : ScriptableObject
{
    public SpritesLine block;

    [Space, Header("Tracks")] public SpritesLine trackHorizontal;
    
    public SpritesLine trackVertical;
}