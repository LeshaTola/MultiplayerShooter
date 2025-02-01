using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace App.Scripts.Modules.CustomToggles
{
    public class ToggleCustom : MonoBehaviour, IPointerClickHandler
    {
        [Serializable]
        public class ToggleCustomEvent : UnityEvent<bool, int>
        {
        }

        [SerializeField] public bool _isInteractable;
        [SerializeField] private bool _isOn;
        [SerializeField] private ToggleGroupCustom _toggleGroup;

        [SerializeField] private bool _isOneState;

        [SerializeField] private GameObject _onStateGO;
        [SerializeField] private GameObject _offStateGO;

        [SerializeField] private Image _image;
        [SerializeField] private Sprite _onStateSprite;
        [SerializeField] private Sprite _offStateSprite;

        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private bool _isChangeImageColor;
        [SerializeField] private int _onSize = 36;
        [SerializeField] private int _offSize = 20;
        [SerializeField] private Color _onStateColor;
        [SerializeField] private Color _offStateColor;

        public ToggleCustomEvent OnValueChanged = new ToggleCustomEvent();
        [field:SerializeField] public int Index { get; private set; }

        private void OnEnable()
        {
            OnValueChanged.AddListener((bool x, int index) => Set(x, Index));
            SetToggleGroup(_toggleGroup, false);
        }

        private void OnDisable()
        {
            OnValueChanged.RemoveListener((bool x, int index) => Set(x, Index));
            SetToggleGroup(null, false);
        }

        public void SetIndex(int index)
        {
            Index = index;
        }

        private void OnValidate()
        {
            PlayEffect(_isOn);
        }

        public bool IsOn
        {
            get { return _isOn; }

            set { Set(value, Index); }
        }


        private void Set(bool value, int index, bool sendCallback = true)
        {
            if(_isInteractable) return;
            if (_isOn == value)
                return;
            _isOn = value;
            if (_toggleGroup != null && _toggleGroup.isActiveAndEnabled && IsActive())
            {
                if (_isOn || (!_toggleGroup.AnyTogglesOn() && !_toggleGroup.IsAllowSwitchOff))
                {
                    _isOn = true;
                    _toggleGroup.NotifyToggleOn(this, sendCallback);
                }
            }

            PlayEffect(_isOn);

             if (sendCallback)
             {
                OnValueChanged?.Invoke(_isOn, Index);
             }
        }

        private void PlayEffect(bool value)
        {
            if (_isOneState)
            {
                _onStateGO.SetActive(value);
            }

            if (_onStateGO != null )
            {
                _onStateGO.SetActive(value);
            }

            if (_offStateGO != null)
            {
                _offStateGO.SetActive(!value);
            }

            if (_image != null)
            {
                if (_offStateSprite != null && _onStateSprite != null)
                {
                    _image.sprite = value ? _onStateSprite : _offStateSprite;
                }

                if (_isChangeImageColor)
                {
                    _image.color = value ? _onStateColor : _offStateColor;
                }
            }

            if (_text != null)
            {
                _text.color = value ? _onStateColor : _offStateColor;
                _text.fontSize = value ? _onSize : _offSize;
            }
        }

        public void SetToggleGroup(ToggleGroupCustom newGroup, bool setMemberValue)
        {
            if (_toggleGroup != null)
                _toggleGroup.UnregisterToggle(this);

            if (setMemberValue)
                _toggleGroup = newGroup;

            if (newGroup != null && IsActive())
                newGroup.RegisterToggle(this);

            if (newGroup != null && _isOn && IsActive())
                newGroup.NotifyToggleOn(this,false);
        }

        public void SetIsOnWithoutNotify(bool value)
        {
            Set(value, Index,false);
        }

        private bool IsActive()
        {
            return gameObject.activeSelf && enabled;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Set(!_isOn, Index);
        }
    }
}