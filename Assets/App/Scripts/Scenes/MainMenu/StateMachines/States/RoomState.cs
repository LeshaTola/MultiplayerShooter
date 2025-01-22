using System.Collections.Generic;
using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.MainMenu.RoomsProviders;
using App.Scripts.Scenes.MainMenu.Screens.RoomsViews;
using Cysharp.Threading.Tasks;
using Photon.Realtime;

namespace App.Scripts.Scenes.MainMenu.StateMachines.States
{
    public class RoomState : State
    {
        private readonly RoomsViewPresenter _roomsViewPresenter;
        private readonly RoomsProvider _roomsProviders;

        public RoomState(RoomsViewPresenter roomsViewPresenter, RoomsProvider roomsProviders)
        {
            _roomsViewPresenter = roomsViewPresenter;
            _roomsProviders = roomsProviders;
        }

        public override async UniTask Enter()
        {
            _roomsProviders.OnRoomListUpdated += OnRoomListUpdated;
            OnRoomListUpdated(_roomsProviders.Rooms);
            await _roomsViewPresenter.Show();
        }

        public override async UniTask Exit()
        {
            _roomsProviders.OnRoomListUpdated -= OnRoomListUpdated;
            await _roomsViewPresenter.Hide();
        }

        private void OnRoomListUpdated(List<RoomInfo> rooms)
        {
            _roomsViewPresenter.UpdateRoomList(rooms);
        }
    }
}