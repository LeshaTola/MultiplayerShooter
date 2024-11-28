using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.StateMachine;
using App.Scripts.Scenes.MainMenu.StateMachines.States;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;

namespace App.Scripts.Scenes.MainMenu.Screens.RoomsViews
{
    public class RoomsViewPresenter : GameScreenPresenter
    {
        private readonly RoomsView _view;
        private readonly StateMachine _stateMachine;

        private Dictionary<string, RoomInfo> _cachedRoomList = new();

        public RoomsViewPresenter(RoomsView view, StateMachine stateMachine)
        {
            _view = view;
            _stateMachine = stateMachine;
        }

        public override void Initialize()
        {
            _view.CloseAction += OnCloseAction;
            _view.Initialize();
        }

        public override void Cleanup()
        {
            _view.CloseAction -= OnCloseAction;
            _view.Initialize();
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

            _view.UpdateRoomListUI(_cachedRoomList.Values.ToList(), (info)=>JoinRoom(info.Name));
        }

        private void JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }

        private async void OnCloseAction()
        {
            await _stateMachine.ChangeState<MainState>();
        }
    }
}