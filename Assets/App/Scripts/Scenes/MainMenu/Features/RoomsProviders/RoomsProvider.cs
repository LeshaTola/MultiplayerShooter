using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.RoomsProviders
{
    public class RoomsProvider : MonoBehaviourPunCallbacks
    {
        public event Action<List<RoomInfo>> OnRoomListUpdated;

        public List<RoomInfo> Rooms { get; private set; } = new();

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Rooms = roomList;
            OnRoomListUpdated?.Invoke(Rooms);
            
            Debug.Log($"UpdateRoomList {PhotonNetwork.CloudRegion}");
        }
    }
}