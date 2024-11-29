using System;
using UnityEngine;

namespace App.Scripts.Features.Input
{
    public class GameInputProvider
    {
        public event Action<int> OnNumber;

        public event Action OnEsc;
        public event Action OnR;
        public event Action OnE;
        
        public event Action OnLeftMouse;
        public event Action OnSpace;

        private GameInput input;

        public GameInputProvider()
        {
            input = new GameInput();
            input.Character.Enable();

            input.Character.ESC.performed += (data) => OnEsc?.Invoke();
            input.Character.R.performed += (data) => OnR?.Invoke();
            input.Character.E.performed += (data) => OnE?.Invoke();

            input.Character.Key1.performed += (data) => OnNumber?.Invoke(1);
            input.Character.Key2.performed += (data) => OnNumber?.Invoke(2);
            input.Character.Key3.performed += (data) => OnNumber?.Invoke(3);
            input.Character.Key4.performed += (data) => OnNumber?.Invoke(4);
            input.Character.Key5.performed += (data) => OnNumber?.Invoke(5);

            input.Character.Jump.performed += (data) => OnSpace?.Invoke();
            input.Character.Attack.performed += (data) => OnLeftMouse?.Invoke();
        }
        
        public Vector2 GetMovementNormalized()
        {
            Vector2 inputVector = input.Character.Move.ReadValue<Vector2>();
            inputVector = inputVector.normalized;
            return inputVector;
        }

        public Vector2 GetMouseLook()
        {
            var mouseLook = input.Character.MouseLook.ReadValue<Vector2>();
            return mouseLook;
        }
    }
}