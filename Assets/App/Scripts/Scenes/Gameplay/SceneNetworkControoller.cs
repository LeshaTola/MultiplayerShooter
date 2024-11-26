using System;
using App.Scripts.Scenes.Gameplay.Stats;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace App.Scripts.Scenes.Gameplay
{
    public class RoomController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Controller.Player _player;
        [SerializeField] private HealthBarUI _healthBarUI;

        private void Start()
        {
            var player
                = PhotonNetwork.Instantiate(_player.gameObject.name, Vector3.zero, Quaternion.identity);

            var health = player.GetComponent<Health>();
            _healthBarUI.Init(health);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("MainMenu");
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
    }
}
