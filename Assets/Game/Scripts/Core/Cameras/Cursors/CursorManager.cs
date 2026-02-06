using System;
using UnityEngine;
using UnityEngine.XR;

namespace Game.Scripts.Core.Cameras.Cursors
{
    public class CursorManager
    {
        private CursorState _previousCursorState;
        
        public CursorManager(CursorState startState)
        {
            var cursorState = startState;
            
            HandleCursorState(cursorState);
        }

        public void Previous()
        {
            HandleCursorState(_previousCursorState);

            _previousCursorState = CursorState.Active;
        }
        
        public void Lock()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Unlock()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void HandleCursorState(CursorState state)
        {
            switch (state)
            {
                case CursorState.Active:
                    Unlock();
                    break;
                case CursorState.Inactive:
                    Lock();
                    break;
                case CursorState.None:
                    Debug.LogWarning("Trying to return to previous state while its none");
                    break;
            }
        }
    }
}