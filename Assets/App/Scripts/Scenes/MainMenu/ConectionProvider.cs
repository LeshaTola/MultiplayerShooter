using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu
{
    public class ConectionProvider : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Button _createRoomButton;
        [SerializeField] private Button _joinRoomButton;
        [SerializeField] private TextMeshProUGUI _statusText;
        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("ConnectedToServer");
            _createRoomButton.onClick.AddListener(CreateRoom);
            _joinRoomButton.onClick.AddListener(JoinRoom);
        }

        public void CreateRoom()
        {
            PhotonNetwork.CreateRoom(null, null, null);
        }

        public void JoinRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("Gameplay");
        }
    }
}
