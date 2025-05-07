using System;
using System.Collections.Generic;
using App.Scripts.Modules.CustomToggles;
using App.Scripts.Scenes.MainMenu.Features.Inventory.GameInventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace App.Scripts.Features.Input
{
    public class MobileInputView : MonoBehaviour
    {
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _rButton;
        [SerializeField] private Button _enterButton;
        [SerializeField] private Button _tabButton;
        [FormerlySerializedAs("_leftMouseButton")] [SerializeField] private List<Button> _leftMouseButtons;
        [FormerlySerializedAs("_rightMouseButton")] [SerializeField] private List<Button> _rightMouseButtons;
        [SerializeField] private Button _spaceButton;
        [SerializeField] private Joystick _joystick;
        [SerializeField] private RectTransform _swipeZone;
        [SerializeField] private ToggleCustom _autoAttackToggle;

        public event Action OnPause;
        public event Action OnR;
        public event Action OnEnter;
        public event Action OnTabPerformed;
        public event Action OnTabCanceled;
        public event Action OnLeftMouseStarted;
        public event Action OnLeftMouseCanceled;
        public event Action OnRightMouseStarted;
        public event Action OnRightMouseCanceled;
        public event Action OnSpace;
        public event Action<bool> OnAutoClicked;


        private GameInventoryViewPresenter _gameInventoryViewPresenter;
        private Vector2 _lastTouchPosition;
        private Vector2 _swipeDelta;
        private bool _isSwiping;

        private void Awake()
        {
            AddButtonListener(_pauseButton, () => OnPause?.Invoke());
            AddButtonListener(_enterButton, () => OnEnter?.Invoke());
            AddButtonListener(_rButton, () => OnR?.Invoke());
            foreach (var button in _leftMouseButtons)
            {
                AddButtonListener(button, () => OnLeftMouseStarted?.Invoke(), () => OnLeftMouseCanceled?.Invoke());
            }
            foreach (var button in _rightMouseButtons)
            {            
                AddButtonListener(button, () => OnRightMouseStarted?.Invoke(), () => OnRightMouseCanceled?.Invoke());
            }

            AddButtonListener(_tabButton, () => OnTabPerformed?.Invoke(), () => OnTabCanceled?.Invoke());
            AddButtonListener(_spaceButton, () => OnSpace?.Invoke());
            _autoAttackToggle.OnValueChanged.AddListener((value,_)=> OnAutoClicked?.Invoke(value));  
        }

        private void Update()
        {
            HandleSwipe();
        }

        public void SetAuto(bool isAuto)
        {
            _autoAttackToggle.SetIsOnWithoutNotify(isAuto);
        }
    
        public Vector2 GetJoystickInput() => _joystick ? _joystick.Direction : Vector2.zero;
    
        public Vector2 GetSwipeDelta() => _swipeDelta * 0.1f;
    
        private void HandleSwipe()
        {
            _swipeDelta = Vector2.zero;

            Touch? touch = GetFirstTouchInZone();
            if (touch.HasValue)
            {
                ProcessSwipe(touch.Value);
            }
        }

        private Touch? GetFirstTouchInZone()
        {
            foreach (var touch in UnityEngine.Input.touches)
            {
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        _swipeZone,
                        touch.position,
                        null,
                        out var localPoint) && 
                    _swipeZone.rect.Contains(localPoint))
                {
                    return touch;
                }
            }
            return null;
        }

        private void ProcessSwipe(Touch touch)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _lastTouchPosition = touch.position;
                    _isSwiping = true;
                    break;
                case TouchPhase.Moved:
                    if (_isSwiping)
                    {
                        _swipeDelta = touch.position - _lastTouchPosition;
                        _lastTouchPosition = touch.position;
                    }
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    _isSwiping = false;
                    break;
            }
        }

        
        private void AddButtonListener(Button button, Action onPress, Action onRelease = null)
        {
            if (button == null)
            {
                return;
            }
            var trigger = PressActionRegistration(button, onPress);

            if (onRelease == null)
            {
                return;
            }
            ReleaseTriggerRegistration(onRelease, trigger);
        }
        
        private void ReleaseTriggerRegistration(Action onRelease, EventTrigger trigger)
        {
            var entryUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
            entryUp.callback.AddListener(_ => onRelease?.Invoke());
            trigger.triggers.Add(entryUp);
        }

        private EventTrigger PressActionRegistration(Button button, Action onPress)
        {
            var trigger = button.gameObject.AddComponent<EventTrigger>();
            var entryDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            entryDown.callback.AddListener(_ => onPress?.Invoke());
            trigger.triggers.Add(entryDown);
            return trigger;
        }
    }
}