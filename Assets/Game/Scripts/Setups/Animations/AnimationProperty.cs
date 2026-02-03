using UnityEngine;

namespace Game.Scripts.Setups.Animations
{
    public class AnimationProperty<T> : ScriptableObject
    {
        [field: SerializeField] public float Duration { get; private set; } = 0.2f;
        [field: SerializeField] public T Target{ get; private set; }
        [field: SerializeField] public AnimationCurve Curve { get; private set; }
    }
}