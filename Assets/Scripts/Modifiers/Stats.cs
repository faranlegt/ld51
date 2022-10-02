using System;

namespace Modifiers
{
    [Serializable]
    public class Stats
    {
        public float speed;

        public Stats Copy() =>
            new() {
                speed = speed
            };
    }
}