using Game.Scripts.Core;
using Game.Scripts.UI.Controls;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.Utilities.MonoDebugs
{
    public class CameraDebug : MonoBehaviour
    {
        [SerializeField] private CinemachineBrain _brain;
        
        [SerializeField] private CinemachineCamera _cameraA;
        [SerializeField] private CinemachineCamera _cameraB;

        [SerializeField] private InputActionReference _referenceA;
        [SerializeField] private InputActionReference _referenceB;
        [SerializeField] private InputActionReference _referenceC;

        private void Update()
        {
            if (_referenceA.action.WasPerformedThisFrame())
            {
                G.Camera.SwitchCut(_cameraA);
            }

            if (_referenceB.action.WasPerformedThisFrame())
            {
                G.Camera.SwitchSoft(_cameraB);
            }
            
            if (_referenceB.action.WasPerformedThisFrame())
            {
                G.Camera.SetBrain(_brain);
            }
        }
    }
}