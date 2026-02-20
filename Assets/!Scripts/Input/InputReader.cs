using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Capstone
{
    [Serializable]
    public class InputReader : InputSystem_Actions.IPlayerActions
    {
        InputSystem_Actions actions;
        public void Enable()
        {
            actions = new InputSystem_Actions();
            actions.Player.SetCallbacks(this);
            actions.Enable();
        }

        public void Disable()
        {
            actions.Player.RemoveCallbacks(this);
            actions.Disable();
        }

        public UnityEvent<Vector2> onMove;
        public void OnMove(InputAction.CallbackContext context)
        {
            onMove?.Invoke(context.ReadValue<Vector2>());
        }

        public UnityEvent onInteract;
        public void OnInteract(InputAction.CallbackContext context)
        {
            if(!context.started)return;
            onInteract?.Invoke();
        }

        public UnityEvent onCrouch;
        public void OnCrouch(InputAction.CallbackContext context)
        {
        }

        public UnityEvent onJump;
        public void OnJump(InputAction.CallbackContext context)
        {
            if(!context.started)return;
            onJump?.Invoke();
        }
        
        public UnityEvent onSprint;
        public void OnSprint(InputAction.CallbackContext context)
        {
            if(!context.started)return;
            onSprint?.Invoke();
        }
        
        public UnityEvent<int> onAbility;
        public void OnAbility(InputAction.CallbackContext context)
        {
            if(!context.started)return;
            onAbility?.Invoke((int)context.ReadValue<float>());
        }

        public UnityEvent onCameraChange;
        public void OnChangeCamera(InputAction.CallbackContext context)
        {
            if(!context.started)return;
            onCameraChange?.Invoke();
        }

        public UnityEvent onMenu;
        public void OnMenu(InputAction.CallbackContext context)
        {
            if(!context.started) return;
            onMenu?.Invoke();
        }
    }
}
