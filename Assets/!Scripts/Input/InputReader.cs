using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Capstone
{
    [Serializable]
    public class InputReader : InputSystem_Actions.IPlayerActions
    {
        public UnityEvent<Vector2> onMove;

        public UnityEvent onInteract;

        public UnityEvent onCrouch;

        public UnityEvent onJump;

        public UnityEvent onSprint;

        public UnityEvent<int> onAbility;

        public UnityEvent onCameraChange;

        public UnityEvent onMenu;
        public UnityEvent onCloseMenu;

        public UnityEvent<Vector2> onMousePosition;

        public UnityEvent onCameraLock;

        public UnityEvent onShop;
        private InputSystem_Actions actions;

        public void OnMove(InputAction.CallbackContext context)
        {
            onMove?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            onInteract?.Invoke();
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            onJump?.Invoke();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.performed) return;
            onSprint?.Invoke();
        }

        public void OnAbility(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            onAbility?.Invoke((int)context.ReadValue<float>());
        }

        public void OnChangeCamera(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            onCameraChange?.Invoke();
        }

        public void OnMenu(InputAction.CallbackContext context)
        {
            if (!context.started) return;

            if (MenuManager.instance.currentMenu == MenuManager.Menu.None)
                onMenu?.Invoke();
            else
                onCloseMenu?.Invoke();
        }

        public void OnMousePosition(InputAction.CallbackContext context)
        {
            onMousePosition?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnCameraLock(InputAction.CallbackContext context)
        {
            if (context.performed) return;
            onCameraLock?.Invoke();
        }

        public void OnShop(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            onShop?.Invoke();
        }

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
    }
}