using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.MainMenu.Features.RoomsProviders;
using App.Scripts.Scenes.MainMenu.Features.Screens.TopViews;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.RoomsViews
{
    public class RoomsViewElementPresenter : GameScreenPresenter, IInitializable, ICleanupable, ITopViewElementPrezenter
    {
        private readonly RoomsView _view;
        private readonly ConnectionProvider _connectionProvider;
        private readonly RoomsProvider _roomsProvider;
        private readonly ILocalizationSystem _localizationSystem;

        private Dictionary<string, RoomInfo> _cachedRoomList = new();

        public RoomsViewElementPresenter(RoomsView view, 
            ConnectionProvider connectionProvider,
            RoomsProvider roomsProvider,
            ILocalizationSystem localizationSystem)
        {
            _view = view;
            _connectionProvider = connectionProvider;
            _roomsProvider = roomsProvider;
            _localizationSystem = localizationSystem;
        }

        public override void Initialize()
        {
            _view.Initialize(_localizationSystem);
            _view.OnQuickGameButtonClicked += OnQuickGameButtonClicked;
            _roomsProvider.OnRoomListUpdated += UpdateRoomList;
        }

        private void OnQuickGameButtonClicked()
        {
            _connectionProvider.QuickGame();
        }

        public override void Cleanup()
        {
            _view.Cleanup();
            _view.OnQuickGameButtonClicked -= OnQuickGameButtonClicked;
            _roomsProvider.OnRoomListUpdated -= UpdateRoomList;
        }

        public override async UniTask Show()
        {
           await _view.Show();
        }

        public override async UniTask Hide()
        {
           await _view.Hide();
        }

        public void UpdateRoomList(List<RoomInfo> roomList)
        {
            if (roomList == null)
            {
                return;
            }
            foreach (var room in roomList)
            {
                if (room.RemovedFromList)
                {
                    _cachedRoomList.Remove(room.Name);
                }
                else
                {
                    _cachedRoomList[room.Name] = room;
                }
            }

            _view.UpdateRoomListUI(_cachedRoomList.Values.ToList(), (info, password)=>JoinRoom(info, password));
        }

        private void JoinRoom(RoomInfo room, string myPassword)
        {
            if (room.CustomProperties.TryGetValue("Password", out object password) && password != null)
            {
                if (password.ToString() == myPassword)
                {
                    PhotonNetwork.JoinRoom(room.Name);
                }
                else
                {
                    Debug.LogError("Incorrect password.");
                }
                return;
            }
            PhotonNetwork.JoinRoom(room.Name);
        }
    }
}