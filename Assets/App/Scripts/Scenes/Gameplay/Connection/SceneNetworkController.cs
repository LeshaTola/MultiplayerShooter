using App.Scripts.Features.Match.Maps;
using App.Scripts.Scenes.Gameplay.Chat;
using App.Scripts.Scenes.Gameplay.Player.Factories;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using IInitializable = App.Scripts.Modules.StateMachine.Services.InitializeService.IInitializable;

namespace App.Scripts.Scenes.Gameplay
{
    public class SceneNetworkController : MonoBehaviourPunCallbacks, IInitializable
    {
        private MapsProvider _mapsProvider;
        private PlayerProvider _playerProvider;
        private int _mapId;
        private ChatViewPresenter _chatViewPresenter;

        [Inject]
        public void Constructor(MapsProvider mapsProvider, 
            PlayerProvider playerProvider,
            ChatViewPresenter chatViewPresenter)
        {
            _chatViewPresenter = chatViewPresenter;
            _mapsProvider = mapsProvider;
            _playerProvider = playerProvider;
        }

        public void Initialize()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var mapObject = PhotonNetwork.InstantiateRoomObject(_mapsProvider.Map.name, Vector3.zero, Quaternion.identity, 0);
                photonView.RPC(nameof(SetMapId), RpcTarget.AllBuffered, mapObject.GetComponent<PhotonView>().ViewID);
            }
        }

        public void KillRoom()
        {
            PhotonNetwork.CurrentRoom.IsVisible = false;
            foreach (var player in PhotonNetwork.PlayerListOthers)
            {
                PhotonNetwork.CloseConnection(player);
            }
        }

        [PunRPC]
        public void SetMapId(int mapId)
        {
            _mapId = mapId;
            var mapObject = PhotonView.Find(_mapId);
            var map = mapObject?.GetComponent<Map>();
            if (map == null)
            {
                map = FindObjectOfType<Map>();
            }
            
            _mapsProvider.CurrentMap = map;
            
            _playerProvider.SetSpawnPoints(_mapsProvider.CurrentMap.SpawnPoints);
        }

        public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
        {
            if (newMasterClient.IsLocal)
            {
                photonView.RPC(nameof(SetMapId), RpcTarget.AllBuffered, _mapId);
            }
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("MainMenu");

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _chatViewPresenter.SendJoinMessage(newPlayer.NickName);
            }
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _chatViewPresenter.SendLeaveMessage(otherPlayer.NickName);
            }
        }
        
    }
}