using App.Scripts.Scenes.Gameplay.Controller;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace App.Scripts.Scenes.Gameplay.Weapons
{
    public class WeaponView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _ammoText;
        [SerializeField] private Image _ammoImage;
        [SerializeField] private Image _reloadImage;

        private WeaponProvider _weaponProvider;
        private Weapon _currentWeapon;
        private Tweener _tween;

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
            if (!weapon)
            {
                return;
            }
            
            if (_currentWeapon)
            {
                _currentWeapon.OnAmmoChanged -= Setup;
                _currentWeapon.OnReloadStarted -= OnReloadStarted;
                _currentWeapon.OnReloadFinised -= OnReloadFinised;
            }
            
            _currentWeapon = weapon;
            _currentWeapon.OnAmmoChanged += Setup;
            _currentWeapon.OnReloadStarted += OnReloadStarted;
            _currentWeapon.OnReloadFinised += OnReloadFinised;
            Setup(_currentWeapon.CurrentAmmoCount, _currentWeapon.Config.MaxAmmoCount);
        }

        private void OnReloadFinised()
        {
            _tween.Kill();
            
            _ammoImage.gameObject.SetActive(true);
            _ammoText.gameObject.SetActive(true);
            _reloadImage.gameObject.SetActive(false);
        }

        private void OnReloadStarted(float reloadTime)
        {
            _ammoImage.gameObject.SetActive(false);
            _ammoText.gameObject.SetActive(false);
            _reloadImage.gameObject.SetActive(true);
            
            _reloadImage.fillAmount = 0;
            _tween = DOVirtual.Float(0,1,reloadTime,value =>
            {
                _reloadImage.fillAmount = value;
            });
        }

        private void Setup(int current, int max)
        {
            OnReloadFinised();
            _ammoText.text = $"{current}/{max}";
            _ammoImage.fillAmount = (float)current / max;
        }
    }
}
