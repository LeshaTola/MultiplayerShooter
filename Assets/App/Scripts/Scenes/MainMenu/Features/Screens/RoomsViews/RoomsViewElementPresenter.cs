﻿using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features;
using App.Scripts.Features.GameMods.Providers;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.Sounds.Providers;
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
        private readonly ISoundProvider _soundProvider;

        private readonly Dictionary<string, RoomInfo> _cachedRoomList = new();
        private readonly GameModProvider _gameModProvider;

        public RoomsViewElementPresenter(RoomsView view, 
            ConnectionProvider connectionProvider,
            RoomsProvider roomsProvider,
            ILocalizationSystem localizationSystem, 
            ISoundProvider soundProvider, GameModProvider gameModProvider)
        {
            _view = view;
            _connectionProvider = connectionProvider;
            _roomsProvider = roomsProvider;
            _localizationSystem = localizationSystem;
            _soundProvider = soundProvider;
            _gameModProvider = gameModProvider;
        }

        public override void Initialize()
        {
            _view.Initialize(_localizationSystem);
            _view.OnQuickGameButtonClicked += OnQuickGameButtonClicked;
            _view.OnCreateRoomButtonClicked += OnCreateRoomButtonClicked;
            _roomsProvider.OnRoomListUpdated += UpdateRoomList;
        }

        public override void Cleanup()
        {
            _view.Cleanup();
            _view.OnQuickGameButtonClicked -= OnQuickGameButtonClicked;
            _view.OnCreateRoomButtonClicked -= OnCreateRoomButtonClicked;
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

            if (roomList.Count == 0)
            {
                _cachedRoomList.Clear();
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
            _soundProvider.PlaySound(_view.ButtonSoundKey);
            if (room.CustomProperties.TryGetValue("GameMode", out var gameMode))
            {
                _gameModProvider.SetGameMod(gameMode.ToString());
            }
            
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
        

        private void OnCreateRoomButtonClicked()
        {
            _soundProvider.PlaySound(_view.ButtonSoundKey);
        }

        private void OnQuickGameButtonClicked()
        {
            _soundProvider.PlaySound(_view.ButtonSoundKey);
            _connectionProvider.QuickGame();
        }
    }
}