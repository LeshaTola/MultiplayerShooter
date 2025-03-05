using App.Scripts.Modules;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using App.Scripts.Scenes.Gameplay.Weapons;
using Cinemachine;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Player
{
    public class LocalPlayerController : MonoBehaviour
    {
        [SerializeField] private PhotonView _photonView;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private Player _player;
        [SerializeField] private HealthBarUI _healthBarUI;
        [SerializeField] private WeaponProvider _weaponProvider;
        [SerializeField] private GameObject _view;

        public void Start()
        {
            if (_photonView.IsMine)
            {
                foreach (var weapon in _weaponProvider.Weapons)
                {
                    if (!weapon)
                    {
                        continue;
                    }
                    ChangeLayerRecursively.SetLayerRecursively(weapon.transform, "Weapon");
                }
                _healthBarUI.gameObject.SetActive(false);
                //_view.gameObject.SetActive(false);
                return;
            }
			
            _virtualCamera.enabled = false;
            _player.enabled = false;
        }
    }
}