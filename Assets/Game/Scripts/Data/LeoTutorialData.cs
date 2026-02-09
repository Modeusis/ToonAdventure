using UnityEngine;

namespace Game.Scripts.Data
{
    public class LeoTutorialData
    {
        public Transform BalconyTransform { get; private set; }
        public Transform ToyFoundTransform { get; private set; }
        
        public LeoTutorialData(Transform balconyTransform, Transform toyFoundTransform)
        {
            BalconyTransform = balconyTransform;
            ToyFoundTransform = toyFoundTransform;
        }
    }
}