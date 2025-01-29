using System;
using System.Collections.Generic;
using App.Scripts.Modules.PopupAndViews.Views;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.RoomsViews
{
    public class RoomsView : AnimatedView
    {
        public event Action OnQuickGameButtonClicked;
        
        [SerializeField] private RectTransform _container;
        [SerializeField] private RoomView _prefab;

        [SerializeField] private Button _quickGameButton;
        
        [Header("Create Room")]
        [SerializeField] private Button _createRoomButton;
        [SerializeField] private CreateRoomView _createRoomView;
        [SerializeField] private InputPasswordView _inputPasswordView;
        
        public void Initialize()
        {
            _createRoomButton.onClick.AddListener(ShowCreateRoom);
            _quickGameButton.onClick.AddListener(()=>OnQuickGameButtonClicked?.Invoke());
        }

        public void Cleanup()
        {
            _createRoomButton.onClick.RemoveAllListeners();
            _quickGameButton.onClick.RemoveAllListeners();
        }

        public void UpdateRoomListUI(List<RoomInfo> rooms, Action<RoomInfo, string> action)
        {
            foreach (Transform child in _container.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (var room in rooms)
            {
                var roomItem = Instantiate(_prefab, _container.transform);
                roomItem.Setup(room,"PVP", ()=>OnJoinRoom(room, action));
            }
        }

        private async void OnJoinRoom(RoomInfo room,Action<RoomInfo, string> action)
        {
            if (room.CustomProperties.TryGetValue("Password", out object password) && password != null)
            {
                _inputPasswordView.Setup((myPassword) =>
                {
                    action?.Invoke(room, myPassword);
                });
                await _inputPasswordView.Show();
                return;
            }
            action?.Invoke(room, "");
        }

        private async void ShowCreateRoom()
        {
            await _createRoomView.Show();
        }
    }
}