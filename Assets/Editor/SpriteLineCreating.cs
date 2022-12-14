using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class SpriteLineCreating : MonoBehaviour
    {
        [MenuItem("Assets/Create/r2d3/Sprites line")]
        public static void CreateSpritesLine()
        {
            var asset = ScriptableObject.CreateInstance<SpritesLine>();
            var sprites = new List<Sprite>();

            foreach (var selected in Selection.objects)
            {
                if (selected is Sprite s)
                {
                    sprites.Add(s);
                }
            }

            asset.sprites = sprites.ToArray();

            ProjectWindowUtil.CreateAsset(asset, "Sprites Line.asset");
        }
    }
}