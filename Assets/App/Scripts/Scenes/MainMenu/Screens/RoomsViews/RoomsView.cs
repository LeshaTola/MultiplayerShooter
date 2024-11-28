using System;
using System.Collections.Generic;
using App.Scripts.Modules.PopupAndViews.Views;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Screens.RoomsViews
{
    public class RoomsView : AnimatedView
    {
        public event Action CloseAction;

        [SerializeField] private RectTransform _container;
        [SerializeField] private RoomView _prefab;
        [SerializeField] private Button _closeButton;

        [Header("Create Room")]
        [SerializeField] private Button _createRoomButton;
        [SerializeField] private CreateRoomView _createRoomView;

        public void Initialize()
        {
            _closeButton.onClick.AddListener(() => CloseAction?.Invoke());
            _createRoomButton.onClick.AddListener(ShowCreateRoom);
        }

        public void Cleanup()
        {
            _closeButton.onClick.RemoveAllListeners();
            _createRoomButton.onClick.RemoveAllListeners();
        }

        public void UpdateRoomListUI(List<RoomInfo> rooms, Action<RoomInfo> action)
        {
            foreach (Transform child in _container.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (var room in rooms)
            {
                var roomItem = Instantiate(_prefab, _container.transform);
                roomItem.Setup(room.Name, room.PlayerCount, room.MaxPlayers, () => action?.Invoke(room));
            }
        }

        private async void ShowCreateRoom()
        {
            await _createRoomView.Show();
        }
    }
}