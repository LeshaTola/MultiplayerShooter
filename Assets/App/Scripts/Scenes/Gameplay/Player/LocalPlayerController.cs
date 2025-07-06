using App.Scripts.Modules;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using App.Scripts.Scenes.Gameplay.Weapons;
using App.Scripts.Scenes.Gameplay.Weapons.Animations;
using Cinemachine;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Scenes.Gameplay.Player
{
    public class LocalPlayerController : MonoBehaviour
    {
        [SerializeField] private PhotonView _photonView;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private Player _player;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private HealthBarUI _healthBarUI;
        [SerializeField] private WeaponProvider _weaponProvider;

        [SerializeField] private GameObject _weaponHolder;
        [SerializeField] private Transform _weaponHolderPosition;
        [SerializeField] private SwayNBobScript _sway;

        
        [SerializeField] [ValueDropdown("GetLayerNames")]
        public string _selectedLayer;

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
                _player.PlayerVisual.Hide();
                _player.gameObject.layer = LayerMask.NameToLayer(_selectedLayer);
                _weaponHolder.transform.SetParent(_weaponHolderPosition);
                _weaponHolder.transform.localScale = Vector3.one;
                _weaponHolder.transform.localPosition = Vector3.zero;
                return;
            }

            _virtualCamera.enabled = false;
            _player.enabled = false;
            _playerMovement.enabled = false;
            _sway.enabled = false;
        }
        
        private string[] GetLayerNames()
        {
            string[] layers = new string[32]; // В Unity всего 32 слоя
            for (int i = 0; i < 32; i++)
            {
                layers[i] = LayerMask.LayerToName(i);
            }
            return layers;
        }
    }
}