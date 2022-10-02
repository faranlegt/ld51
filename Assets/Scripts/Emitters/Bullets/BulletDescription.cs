using UnityEngine;

namespace Emitters.Bullets
{
    [CreateAssetMenu(fileName = "Bullet", menuName = "Snakur/Bullet", order = 0)]
    public class BulletDescription : ScriptableObject
    {
        public Sprite sprite;

        public float speed;
    }
}