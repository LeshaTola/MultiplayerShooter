using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace __Project.Scripts.Helpers
{
    public class ToggleGroupCustom : MonoBehaviour
    {
        [SerializeField] private bool _allowSwitchOff = false;
        private List<ToggleCustom> _toggles = new List<ToggleCustom>();

        public bool IsAllowSwitchOff => _allowSwitchOff;


        private void Start()
        {
            EnsureValidState();
        }

        private void EnsureValidState()
        {
            if (!_allowSwitchOff && !AnyTogglesOn() && _toggles.Count != 0)
            {
                var toggle = _toggles.First(_ => _.Index == 0);
                toggle.IsOn = true;
                NotifyToggleOn(toggle);
            }

            List<ToggleCustom> activeToggles = ActiveToggles();

            if (activeToggles.Count() > 1)
            {
                ToggleCustom firstActive = GetFirstActiveToggle();

                foreach (ToggleCustom toggle in activeToggles)
                {
                    if (toggle == firstActive)
                    {
                        continue;
                    }

                    toggle.IsOn = false;
                }
            }
        }

        public bool AnyTogglesOn()
        {
            return _toggles.Find(x => x.IsOn) != null;
        }

        private ToggleCustom GetFirstActiveToggle()
        {
            List<ToggleCustom> activeToggles = ActiveToggles();
            return activeToggles.Count() > 0 ? activeToggles.First() : null;
        }


        private List<ToggleCustom> ActiveToggles()
        {
            return _toggles.Where(x => x.IsOn).ToList();
        }

        public void NotifyToggleOn(ToggleCustom toggle, bool sendCallback = true)
        {
            ValidateToggleIsInGroup(toggle);
            for (var i = 0; i < _toggles.Count; i++)
            {
                if (_toggles[i] == toggle)
                    continue;

                if (sendCallback)
                    _toggles[i].IsOn = false;
                else
                    _toggles[i].SetIsOnWithoutNotify(false);
            }
        }

        private void ValidateToggleIsInGroup(ToggleCustom toggle)
        {
            if (toggle == null || !_toggles.Contains(toggle))
                throw new ArgumentException(string.Format("Custom Toggle {0} is not part of ToggleGroup {1}",
                    new object[] { toggle, this }));
        }

        public void UnRegisterAllToggles()
        {
            _toggles = new List<ToggleCustom>();
        }
        
        public void UnregisterToggle(ToggleCustom toggle)
        {
            if (_toggles.Contains(toggle))
                _toggles.Remove(toggle);
        }

        public void RegisterToggle(ToggleCustom toggle)
        {
            if (!_toggles.Contains(toggle))
                _toggles.Add(toggle);
        }
    }
}