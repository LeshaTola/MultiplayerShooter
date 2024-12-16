using App.Scripts.Scenes.Gameplay.Controller;
using TMPro;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Weapons
{
    public class WeaponView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _ammoText;

        private WeaponProvider _weaponProvider;
        private Weapon _currentWeapon;
            
        public void Initialize(WeaponProvider weaponProvider)
        {
            _weaponProvider = weaponProvider;
            OnWeaponChanged(_weaponProvider.CurrentWeapon);
            _weaponProvider.OnWeaponChanged += OnWeaponChanged;
        }

        public void Cleanup()
        {
            _weaponProvider.OnWeaponChanged -= OnWeaponChanged;
        }

        private void OnWeaponChanged(Weapon weapon)
        {
            if (_currentWeapon != null)
            {
                _currentWeapon.OnAmmoChanged -= Setup;
            }
            _currentWeapon = weapon;
            _currentWeapon.OnAmmoChanged += Setup;
            Setup(_currentWeapon.CurrentAmmoCount, _currentWeapon.Config.MaxAmmoCount);
        }

        private void Setup(int current, int max)
        {
            _ammoText.text = $"{current}/{max}";
        }
    }
}
