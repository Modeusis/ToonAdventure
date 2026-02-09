using UnityEngine;

namespace Game.Scripts.Setups.Animations
{
    public class AnimationProperty<T> : ScriptableObject
    {
        [field: SerializeField] public float Duration { get; private set; } = 0.2f;
        [field: SerializeField] public T Value{ get; private set; }
        [field: SerializeField] public AnimationCurve Ease { get; private set; }
    }
}