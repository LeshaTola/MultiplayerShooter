using System;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot.SelectionProviders;
using UnityEngine;

namespace App.Scripts.Features.Input
{
    public class MobileGameInputProvider : IGameInputProvider
    {
        public event Action<int> OnNumber;
        public event Action OnPause;
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
    
        private readonly MobileInputView _inputView;
    
        public MobileGameInputProvider(MobileInputView inputView, SelectionProvider selectionProvider)
        {
            _inputView = inputView;
            
            selectionProvider.OnWeaponSelectedSlotId += OnWeaponSelected;
                
            _inputView.OnPause += () => OnPause?.Invoke();
            _inputView.OnEnter += () => OnEnter?.Invoke();
            _inputView.OnR += () => OnR?.Invoke();
            _inputView.OnTabPerformed += () => OnTabPerformed?.Invoke();
            _inputView.OnTabCanceled += () => OnTabCanceled?.Invoke();
            _inputView.OnLeftMouseStarted += () => OnLeftMouseStarted?.Invoke();
            _inputView.OnLeftMouseCanceled += () => OnLeftMouseCanceled?.Invoke();
            _inputView.OnRightMouseStarted += () => OnRightMouseStarted?.Invoke();
            _inputView.OnRightMouseCanceled += () => OnRightMouseCanceled?.Invoke();
            _inputView.OnSpace += () => OnSpace?.Invoke();
        }

        private void OnWeaponSelected(int index)
        {
            OnNumber?.Invoke(index + 1);
        }

        public Vector2 GetMovementNormalized() => _inputView.GetJoystickInput();
    
        public Vector2 GetMouseLook() => _inputView.GetSwipeDelta();
    }
}