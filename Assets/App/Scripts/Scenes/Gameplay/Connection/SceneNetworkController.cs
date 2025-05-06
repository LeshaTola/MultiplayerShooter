using System.Threading;
using System.Threading.Tasks;
using App.Scripts.Features.Match.Maps;
using App.Scripts.Scenes.Gameplay.Chat;
using App.Scripts.Scenes.Gameplay.Player.Factories;
using Cysharp.Threading.Tasks;
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

        public async void Initialize()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.InstantiateRoomObject(_mapsProvider.Map.name, Vector3.zero, Quaternion.identity, 0);
            }
            await FindMap();
        }

        private async UniTask FindMap()
        {
            Map map = null;
            int attempts = 0;
            while (map == null && attempts < 10)
            {
                map = FindObjectOfType<Map>();
                if (map == null) await UniTask.Delay(200);
                attempts++;
            }
            if (map == null) return;
    
            _mapsProvider.CurrentMap = map;
            _playerProvider.SetSpawnPoints(_mapsProvider.CurrentMap.SpawnPoints);
        }

        public void KillRoom()
        {
            PhotonNetwork.CurrentRoom.IsVisible = false;
            KickAllPlayers();
        }
        
        [PunRPC]
        private void RPC_KickPlayer()
        {
            PhotonNetwork.LeaveRoom();
        }

        private void KickAllPlayers()
        {
            if (!PhotonNetwork.IsMasterClient) return;
    
            photonView.RPC(nameof(RPC_KickPlayer), RpcTarget.Others);
            PhotonNetwork.LeaveRoom();
        }

        /*[PunRPC]
        public void SetMapId(int mapId)
        {
            _mapId = mapId;
            var mapObject = PhotonView.Find(_mapId);
            var map = mapObject?.GetComponent<Map>();
            if (map == null)
            {
                map = FindObjectOfType<Map>();
            }
            if (map == null)
            {
                return;
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
        }*/

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