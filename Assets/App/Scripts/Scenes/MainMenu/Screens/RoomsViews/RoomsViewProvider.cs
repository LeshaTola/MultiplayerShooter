using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;

namespace App.Scripts.Scenes.MainMenu.Screens
{
    public class RoomsViewProvider
    {
        private readonly RoomsView _view;
        private Dictionary<string, RoomInfo> _cachedRoomList = new();

        public RoomsViewProvider(RoomsView view)
        {
            _view = view;
        }

        public void Initialize()
        {
            _view.Initialize();
        }

        public void Cleanup()
        {
            _view.Initialize();
        }

        public void Show()
        {
            _view.Show();
        }

        public void Hide()
        {
            _view.Hide();
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

            _view.UpdateRoomListUI(_cachedRoomList.Values.ToList());
        }
    }
}