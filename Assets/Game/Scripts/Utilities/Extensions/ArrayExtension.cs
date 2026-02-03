using UnityEngine;

namespace Game.Scripts.Utilities.Extensions
{
    public static class ArrayExtension
    {
        public static T GetRandom<T>(this T[] array)
        {
            var random = Random.Range(0, array.Length - 1);
            
            return array[random];
        }
    }
}