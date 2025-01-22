using System;
using UnityEngine;

namespace App.Scripts.Features.Input
{
    public class GameInputProvider
    {
        public event Action<int> OnNumber;

        public event Action OnEsc;
        public event Action OnEnter;
        public event Action OnR;
        public event Action OnE;
        public event Action OnTabPerformed;
        public event Action OnTabCanceled;
        
        public event Action OnLeftMouseStarted;
        public event Action OnLeftMouseCanceled;
        public event Action OnRightMouseStarted;
        public event Action OnRightMouseCanceled;
        public event Action OnSpace;
        public event Action<float> OnScrollWheel;

        private readonly GameInput _input;

        public GameInputProvider()
        {
            _input = new GameInput();
            _input.Character.Enable();

            _input.Character.ESC.performed += (data) => OnEsc?.Invoke();
            _input.Character.Enter.performed += (data) => OnEnter?.Invoke();
            _input.Character.R.performed += (data) => OnR?.Invoke();
            _input.Character.E.performed += (data) => OnE?.Invoke();
            _input.Character.Tab.performed += (data) => OnTabPerformed?.Invoke();
            _input.Character.Tab.canceled += (data) => OnTabCanceled?.Invoke();

            _input.Character.Key1.performed += (data) => OnNumber?.Invoke(1);
            _input.Character.Key2.performed += (data) => OnNumber?.Invoke(2);
            _input.Character.Key3.performed += (data) => OnNumber?.Invoke(3);
            _input.Character.Key4.performed += (data) => OnNumber?.Invoke(4);
            _input.Character.Key5.performed += (data) => OnNumber?.Invoke(5);

            _input.Character.Jump.performed += (data) => OnSpace?.Invoke();
            _input.Character.Attack.started += (data) => OnLeftMouseStarted?.Invoke();
            _input.Character.Attack.canceled += (data) => OnLeftMouseCanceled?.Invoke();
            
            _input.Character.AttackAlternative.started += (data) => OnRightMouseStarted?.Invoke();
            _input.Character.AttackAlternative.canceled += (data) => OnRightMouseCanceled?.Invoke();
            _input.Character.MouseScroll.performed += (data) => OnScrollWheel?.Invoke(data.ReadValue<float>());
        }
        
        public Vector2 GetMovementNormalized()
        {
            Vector2 inputVector = _input.Character.Move.ReadValue<Vector2>();
            inputVector = inputVector.normalized;
            return inputVector;
        }

        public Vector2 GetMouseLook()
        {
            var mouseLook = new Vector2(UnityEngine.Input.GetAxis("Mouse X"), UnityEngine.Input.GetAxis("Mouse Y"));
            // var mouseLook = _input.Character.MouseLook.ReadValue<Vector2>();
            return mouseLook;
        }
    }
}