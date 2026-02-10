using Game.Scripts.Core.Interactions;
using Game.Scripts.Utilities.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.Core.Character.States
{
    public class PuzzleState : PlayerBaseState
    {
        public PuzzleState(Player player) : base(player, PlayerState.Puzzle) { }

        private Camera _camera;
        private IClickable _currentClickable;
        
        public override void Enter()
        {
            _camera = Camera.main;
            _animator.SetSpeed(0);
            G.Cursor.Unlock();
        }

        public override void Update()
        {
            HandleRaycast();
            HandleInput();
        }

        public override void Exit()
        {
            if (_currentClickable != null)
            {
                _currentClickable.OnEndHover();
                _currentClickable = null;
            }
            
            G.Cursor.Lock();
        }

        private void HandleRaycast()
        {
            var ray = _camera.ScreenPointToRay(G.Input.Game.MousePosition.ReadValue<Vector2>());
            IClickable newClickable = null;

            if (Physics.Raycast(ray, out var hit)) 
            {
                hit.collider.TryGetComponent(out newClickable);
            }

            if (_currentClickable == newClickable)
                return;
            
            _currentClickable?.OnEndHover();
            _currentClickable = newClickable;
            _currentClickable?.OnBeginHover();

            if (_currentClickable != null)
            {
                ShowTooltip();
                return;
            }
            
            Hide();
        }

        private void HandleInput()
        {
            if (G.Input.Game.Back.WasPressedThisFrame())
            {
                G.EventBus.Publish(new OnPuzzleStopEvent());
                G.EventBus.Publish(new OnPlayerStateChangeRequest { NewState = PlayerState.Active });
                return;
            }

            if (G.Input.Game.Click.WasPerformedThisFrame())
            {
                _currentClickable?.OnClick();
            }
        }

        private void ShowTooltip()
        {
            var inputBinding = G.Input.Game.Interact.GetBindingDisplayString();
            G.UI.Screens.HUD.DynamicTooltip.Show($"{inputBinding} - кликнуть на кнопку");
        }
        
        private void Hide()
        {
            G.UI.Screens.HUD.DynamicTooltip.Hide();
        }
    }
}