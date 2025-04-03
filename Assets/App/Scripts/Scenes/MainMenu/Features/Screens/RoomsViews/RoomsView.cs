using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.PopupAndViews.Views;
using App.Scripts.Modules.Sounds;
using Photon.Realtime;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.RoomsViews
{
    public class RoomsView : GameScreen
    {
        public event Action OnQuickGameButtonClicked;
        public event Action OnCreateRoomButtonClicked;

        [SerializeField] private RectTransform _container;
        [SerializeField] private RoomView _prefab;

        [SerializeField] private Button _quickGameButton;

        [Header("Create Room")]
        [SerializeField] private Button _createRoomButton;

        [SerializeField] private CreateRoomView _createRoomView;
        [SerializeField] private InputPasswordView _inputPasswordView;
        
        [Header("Audio")]
        [SerializeField] private AudioDatabase _audioDatabase;
        [field: SerializeField,ValueDropdown(@"GetAudioKeys")] public string ButtonSoundKey { get; private set; }


        private ILocalizationSystem _localizationSystem;
        private List<RoomView> _roomItems = new();

        public void Initialize(ILocalizationSystem localizationSystem)
        {
            _localizationSystem = localizationSystem;

            _createRoomButton.onClick.AddListener(ShowCreateRoom);
            _quickGameButton.onClick.AddListener(() => OnQuickGameButtonClicked?.Invoke());
        }

        public override void Cleanup()
        {
            _createRoomButton.onClick.RemoveAllListeners();
            _quickGameButton.onClick.RemoveAllListeners();
        }

        public void UpdateRoomListUI(List<RoomInfo> rooms, Action<RoomInfo, string> action)
        {
            CleanupRoomList();

            foreach (var room in rooms)
            {
                var roomItem = Instantiate(_prefab, _container.transform);
                roomItem.Initialize(_localizationSystem);
                roomItem.Setup(room, () => OnJoinRoom(room, action));
                _roomItems.Add(roomItem);
            }
        }

        private void CleanupRoomList()
        {
            foreach (var roomItem in _roomItems)
            {
                roomItem.Cleanup();
                Destroy(roomItem.gameObject);
            }

            _roomItems.Clear();
        }

        private async void OnJoinRoom(RoomInfo room, Action<RoomInfo, string> action)
        {
            _inputPasswordView.Setup((myPassword) => { action?.Invoke(room, myPassword); });
            if (room.CustomProperties.TryGetValue("Password", out object password) && password != null)
            {
                await _inputPasswordView.Show();
                return;
            }

            action?.Invoke(room, "");
        }

        private async void ShowCreateRoom()
        {
            OnCreateRoomButtonClicked?.Invoke();
            await _createRoomView.Show();
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