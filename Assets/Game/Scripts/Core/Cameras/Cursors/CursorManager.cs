using System;
using UnityEngine;

namespace Game.Scripts.Core.Cameras.Cursors
{
    public class CursorManager
    {
        public CursorManager(CursorState startState)
        {
            var cursorState = startState;
            
            switch (cursorState)
            {
                case CursorState.Active:
                    UnlockCursor();
                    break;
                case CursorState.Inactive:
                    LockCursor();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(cursorState), cursorState, null);
            }
        }

        public void LockCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void UnlockCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}