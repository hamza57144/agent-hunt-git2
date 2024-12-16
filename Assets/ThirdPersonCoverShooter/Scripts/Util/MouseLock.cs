using UnityEngine;

namespace CoverShooter
{
    /// <summary>
    /// Locks mouse cursor inside the game window. 
    /// Locked by pressing left mouse button, unlocked by pressing the escape key.
    /// </summary>
    public class MouseLock : MonoBehaviour
    {
        private bool _isLocked = true;

        private void LateUpdate()
        {
            if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.Escape))
                _isLocked = false;

            if (ControlFreak2.CF2Input.GetMouseButtonDown(0))
                _isLocked = true;

            if (_isLocked)
            {
                ControlFreak2.CFCursor.lockState = CursorLockMode.Locked;
                ControlFreak2.CFCursor.visible = false;
            }
            else
            {
                ControlFreak2.CFCursor.lockState = CursorLockMode.None;
                ControlFreak2.CFCursor.visible = true;
            }
        }
    }
}