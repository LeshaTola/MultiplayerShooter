using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Screens
{
    public class RoomsView : MonoBehaviour
    {
        [SerializeField] private RectTransform _container;
        [SerializeField] private RoomView _prefab;
        [SerializeField] private Button _closeButton;
        
        [Header("Create Room")]
        [SerializeField] private Button _createRoomButton;
        [SerializeField] private CreateRoomView _createRoomView;
    
        public void Initialize()
        {
            _closeButton.onClick.AddListener(Hide);
            _createRoomButton.onClick.AddListener(_createRoomView.Show);
        }

        public void Cleanup()
        {
            _closeButton.onClick.RemoveAllListeners();
            _createRoomButton.onClick.RemoveAllListeners();
        }

        public void Show()
        {
          gameObject.SetActive(true);  
        }

        public void Hide()
        {
            gameObject.SetActive(false);  
        }
        

        public void UpdateRoomListUI(List<RoomInfo> rooms)
        {
            foreach (Transform child in _container.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (var room in rooms)
            {
                var roomItem = Instantiate(_prefab, _container.transform);
                roomItem.Setup(room.Name,room.PlayerCount,room.MaxPlayers,() => JoinRoom(room.Name));
            }
        }

        private void JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }
    }
}
