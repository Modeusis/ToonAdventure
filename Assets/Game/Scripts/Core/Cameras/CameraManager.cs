using Game.Scripts.Setups.Core;
using Unity.Cinemachine;
using UnityEngine;

namespace Game.Scripts.Core.Cameras
{
    public class CameraManager
    {
        private CinemachineBrain _brain;
        
        private CinemachineCamera _currentCamera;
        
        private readonly int _inactivePriority;
        private readonly int _activePriority;

        public CameraManager(CameraManagerSettings settings)
        {
            _inactivePriority = settings.InactivePriority;
            _activePriority = settings.ActivePriority;
        }

        public void SetBrain(CinemachineBrain brain)
        {
            _brain = brain;
        }

        public void SwitchCut(CinemachineCamera camera)
        {
            SwitchToCamera(camera, TransitionType.Cut);
        }
        
        public void SwitchSoft(CinemachineCamera camera, float duration = 1.5f)
        {
            SwitchToCamera(camera, TransitionType.Smooth, duration);
        }
        
        private void SwitchToCamera(CinemachineCamera newCamera, TransitionType type, float duration = 1.5f)
        {
            Debug.Log($"Switching to camera: {newCamera?.name}");
            
            if (_currentCamera == newCamera)
                return;
            
            if (_currentCamera)
            {
                _currentCamera.Priority = _inactivePriority;
            }
            
            ChangeBlending(type, duration);
            
            _currentCamera = newCamera;
            _currentCamera.Priority = _activePriority;
        }

        private void ChangeBlending(TransitionType type, float duration)
        {
            if (!_brain)
            {
                Debug.Log("[CameraManager.ChangeBlending] No brain assigned to CameraManager");
                return;
            }
            
            if (type == TransitionType.Cut)
            {
                _brain.DefaultBlend.Style = CinemachineBlendDefinition.Styles.Cut;
            }
            else
            {
                _brain.DefaultBlend.Style = CinemachineBlendDefinition.Styles.EaseInOut;
                _brain.DefaultBlend.Time = duration;
            }
        }
    }
}