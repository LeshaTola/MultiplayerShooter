using App.Scripts.Scenes.Gameplay.Stats;
using Cinemachine;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Controller
{
    public class LocalPlayerController : MonoBehaviour
    {
        [SerializeField] private PhotonView _photonView;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private Player _player;
        [SerializeField] private HealthBarUI _healthBarUI;

        public  void Start()
        {
            if (_photonView.IsMine)
            {
                _healthBarUI.gameObject.SetActive(false);
                return;
            }
			
            _virtualCamera.enabled = false;
            _player.enabled = false;
        }
    }
}