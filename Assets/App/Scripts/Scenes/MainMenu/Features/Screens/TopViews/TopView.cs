using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.CustomToggles;
using App.Scripts.Modules.Sounds;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.TopViews
{
    public class TopView : GameScreen
    {
        public event Action<int> OnToggleClicked;
        public event Action OnSettingsClicked;
        public event Action OnTutorClicked;
        
        [SerializeField] private AudioDatabase _audioDatabase;
        [field: SerializeField,ValueDropdown(@"GetAudioKeys")] public string ButtonSound { get; private set; }
        [field: SerializeField,ValueDropdown(@"GetAudioKeys")] public string ToggleSound { get; private set; }
        
        [SerializeField] private List<ToggleCustom> _toggles;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _tutorButton;

        public override void Initialize()
        {
            foreach (var toggle in _toggles)
            {
                toggle.OnValueChanged.AddListener(OnToggleChanged);
            }

            _settingsButton.onClick.AddListener(OnSettingsClick);
            _tutorButton.onClick.AddListener(OpenTutor);
        }

        public void SetTab(int index)
        {
            _toggles[index].IsOn = true;
        }

        private void OpenTutor()
        {
            OnTutorClicked?.Invoke();
        }

        private void OnSettingsClick()
        {
            OnSettingsClicked?.Invoke();
        }

        private void OnToggleChanged(bool isActive, int index)
        {
            if (!isActive)
            {
                return;
            }

            OnToggleClicked?.Invoke(index);
        }
        
        public List<string> GetAudioKeys()
        {
            if (_audioDatabase == null)
            {
                return null;
            }
            return _audioDatabase.Audios.Keys.ToList();
        }
    }
}