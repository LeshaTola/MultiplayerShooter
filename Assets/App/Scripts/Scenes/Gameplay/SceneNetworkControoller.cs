using App.Scripts.Features.Match.Maps;
using App.Scripts.Scenes.Gameplay.Player.Factories;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using IInitializable = App.Scripts.Modules.StateMachine.Services.InitializeService.IInitializable;

namespace App.Scripts.Scenes.Gameplay
{
    public class SceneNetworkControoller : MonoBehaviourPunCallbacks, IInitializable
    {
        private MapsProvider _mapsProvider;
        private PlayerProvider _playerProvider;
        private int _mapId;

        [Inject]
        public void Constructor(MapsProvider mapsProvider, PlayerProvider playerProvider)
        {
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

        [PunRPC]
        public void SetMapId(int mapId)
        {
            _mapId = mapId;
            _playerProvider.SetSpawnPoints(PhotonView.Find(_mapId).GetComponent<Map>().SpawnPoints);
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
    }
}