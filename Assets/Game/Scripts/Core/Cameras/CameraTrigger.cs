using UnityEngine;
using Unity.Cinemachine;

namespace Game.Scripts.Core.Cameras
{
    public class CameraTrigger : MonoBehaviour
    {
        [Header("Target Camera")]
        [SerializeField] private CinemachineCamera _cameraToActivate;

        [Header("Transition Settings")]
        [Tooltip("Cut - мгновенно, Smooth - плавно")]
        [SerializeField] private TransitionType _transitionType = TransitionType.Smooth;
    
        [Tooltip("Длительность полета камеры (игнорируется при Cut)")]
        [SerializeField] private float _smoothTime = 1.5f;

        [Header("Tag Settings")]
        [SerializeField] private string _playerTag = "Player";
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(_playerTag))
                return;
            
            if (_transitionType == TransitionType.Smooth)
            {
                G.Camera.SwitchSoft(_cameraToActivate, _smoothTime);
                return;
            }
            
            G.Camera.SwitchCut(_cameraToActivate);
        }
    }
}