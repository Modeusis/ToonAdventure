using Game.Scripts.Setups.Core;
using Game.Scripts.Utilities.StateMachine;
using Unity.Cinemachine;
using UnityEngine;

namespace Game.Scripts.Core.Character.States
{
    public abstract class PlayerBaseState : State<PlayerState>
    {
        protected readonly Player _player;
        protected readonly PlayerAnimationController _animator;
        protected readonly CinemachineInputAxisController _cameraController;
        protected readonly CharacterController _characterController;
        protected readonly PlayerSetup _setup;

        protected PlayerBaseState(Player player, PlayerState stateType)
        {
            _player = player;
            StateType = stateType;
            _animator = player.AnimationController;
            _characterController = player.CharacterController;
            _cameraController = player.CameraInputController;
            _setup = player.Setup;
        }

        public override void Enter() { }
        public override void Exit() { }
        public override void Update() { }
    }
}