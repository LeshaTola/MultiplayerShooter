using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using YG;

namespace App.Scripts.Scenes.Gameplay.Connection
{
    public class ConnectionStatusUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _statusText;

        private void Update()
        {
            if (_statusText != null)
            {
                if (YG2.lang == "en")
                {
                    _statusText.text = $"{PhotonNetwork.NetworkClientState}";
                }
                else if (YG2.lang == "ru")
                {
                    switch (PhotonNetwork.NetworkClientState)
                    {
                        case ClientState.PeerCreated:
                            _statusText.text = "Создание соединения";
                            break;
                        case ClientState.ConnectingToMasterServer:
                            _statusText.text = "Подключение к серверу";
                            break;
                        case ClientState.ConnectedToMasterServer:
                            _statusText.text = "Подключено к серверу";
                            break;
                        case ClientState.JoiningLobby:
                            _statusText.text = "Вход в лобби";
                            break;
                        case ClientState.JoinedLobby:
                            _statusText.text = "В лобби";
                            break;
                        case ClientState.Joining:
                            _statusText.text = "Подключение к комнате";
                            break;
                        case ClientState.Joined:
                            _statusText.text = "В комнате";
                            break;
                        case ClientState.Disconnecting:
                            _statusText.text = "Отключение";
                            break;
                        case ClientState.Disconnected:
                            _statusText.text = "Отключено";
                            break;
                        default:
                            _statusText.text = "Неизвестно";
                            break;
                    }
                }
            }
        }
    }
}