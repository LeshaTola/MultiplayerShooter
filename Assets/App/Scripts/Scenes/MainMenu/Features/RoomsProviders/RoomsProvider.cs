using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

namespace App.Scripts.Scenes.MainMenu.Features.RoomsProviders
{
    public class RoomsProvider : MonoBehaviourPunCallbacks
    {
        public event Action<List<RoomInfo>> OnRoomListUpdated;

        public List<RoomInfo> Rooms { get; private set; }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Rooms = roomList;
            OnRoomListUpdated?.Invoke(Rooms);
        }
    }
}