using UnityEngine;

namespace Game.Scripts.Utilities.Extensions
{
    public static class Vector2Extension
    {
        public static float GetRandomInRange(this Vector2 vector)
        {
            var random = Random.Range(vector.x, vector.y);
            
            return random;
        }
    }
}