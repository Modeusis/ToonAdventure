using Game.Scripts.Core;
using Game.Scripts.Core.Levels;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.Utilities.MonoDebugs
{
    public class LevelDebugger : MonoBehaviour
    {
        [SerializeField] private InputActionReference _referenceA;
        [SerializeField] private InputActionReference _referenceB;
        
        private void Update()
        {
            if (_referenceA.action.WasPerformedThisFrame())
            {
                G.EventBus.Publish(LevelType.Tutorial);
            }

            if (_referenceB.action.WasPerformedThisFrame())
            {
                G.EventBus.Publish(LevelType.Level1);
            }
        }
    }
}