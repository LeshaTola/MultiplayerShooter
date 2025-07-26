using System;
using System.Collections.Generic;
using App.Scripts.Modules.Extensions;
using App.Scripts.Scenes.Gameplay.Player.Configs;
using App.Scripts.Scenes.Gameplay.Weapons;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace App.Scripts.Scenes.Gameplay.Player
{
    public class PlayerAudioProvider : MonoBehaviourPun
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private PlayerAudioConfig _audioConfig;
        [SerializeField] private WeaponProviderBase _weaponProvider;

        [Header("Walking")]
        [SerializeField] private float _stepInterval = 0.5f;
        [SerializeField, ValueDropdown(nameof(GetCategories))] private string _walkingCategory;

        [Header("Jumping")]
        [SerializeField, ValueDropdown(nameof(GetCategories))] private string _jumpingCategory;
        [SerializeField, ValueDropdown(nameof(GetCategories))] private string _landingCategory;
        
        [Header("Damage")]
        [SerializeField, ValueDropdown(nameof(GetCategories))] private string _damageCategory;
        [SerializeField, ValueDropdown(nameof(GetCategories))] private string _destroyCategory;
        
        [Header("Kill")]
        [SerializeField, ValueDropdown(nameof(GetCategories))] private string _hitCategory;
        [SerializeField, ValueDropdown(nameof(GetCategories))] private string _killCategory;

        [Header("Weapons")]
        [SerializeField] private float _switchInterval = 0.05f;
        [SerializeField, ValueDropdown(nameof(GetCategories))] private string _switchWeaponCategory;

        private float _stepTimer;
        private float _switchTimer;
        
        private void Update()
        {
            _stepTimer -= Time.deltaTime;
            _switchTimer -= Time.deltaTime;
            
        }

        public void PlayWalkingSound()
        {
            if (_stepTimer > 0)
            {
                return;   
            }
            RPCPlaySound(_walkingCategory, _audioConfig.AudioClips[_walkingCategory].GetRandomIndex());
            _stepTimer = _stepInterval;
        }
        
        public void PlaySwitchWeaponSound()
        {
            if (_switchTimer > 0)
            {
                return;
            }
            RPCPlaySound(_switchWeaponCategory, _audioConfig.AudioClips[_switchWeaponCategory].GetRandomIndex());
            _switchTimer = _switchInterval;
        }
        
        public void PlayJumpingSound()
        {
            RPCPlaySound(_jumpingCategory, _audioConfig.AudioClips[_jumpingCategory].GetRandomIndex());
        }
        
        public void PlayLandingSound()
        {
            RPCPlaySound(_landingCategory, _audioConfig.AudioClips[_landingCategory].GetRandomIndex());
        }
        
        public void PlayDestroySound()
        {
            photonView.RPC(
                nameof(PlaySoundAtPlayerPos),
                RpcTarget.All,
                _destroyCategory,
                Random.Range(0,_audioConfig.AudioClips[_destroyCategory].Count)
                );
        }
        
        public void PlayDamageSound()
        {
            if (photonView.IsMine)
            {
                PlaySound(_damageCategory, _audioConfig.AudioClips[_damageCategory].GetRandomIndex());
            }
        }

        public void PlayHitSound()
        {
            PlaySound(_hitCategory, _audioConfig.AudioClips[_hitCategory].GetRandomIndex());
        }
        
        public void PlayKillSound()
        {
            PlaySound(_killCategory, _audioConfig.AudioClips[_killCategory].GetRandomIndex());
        }
        
        public void RPCPlayReloadWeaponSound()
        {
            photonView.RPC(nameof(PlayReloadSound), RpcTarget.All);
        }
        
        public void RPCPlayWeaponSound()
        {
            photonView.RPC(nameof(PlayWeaponSound), RpcTarget.All);
        }

        [PunRPC]
        public void PlayWeaponSound()
        {
            var clip = _weaponProvider.CurrentWeapon.Config.ShotSound;
            if (clip == null)
            {
                return;
            }
            
            _audioSource.PlayOneShot(clip);
        }
        
        [PunRPC]
        public void PlayReloadSound()
        {
            var clip = _weaponProvider.CurrentWeapon.Config.ReloadSound;
            if (clip == null)
            {
                return;
            }
            
            _audioSource.PlayOneShot(clip);
        }
        
        public void RPCPlaySound(string category, int soundIndex)
        {
            photonView.RPC(nameof(PlaySound), RpcTarget.All, category, soundIndex);
        }
        
        [PunRPC]
        public void PlaySound(string category, int soundIndex)
        {
            if (!_audioConfig.AudioClips.TryGetValue(category, out var clips))
            {
                Debug.LogError("Audio clip for " + category + " does not exist");
                return;
            }
            _audioSource.PlayOneShot(clips[soundIndex]);
        }
        
        [PunRPC]
        public void PlaySoundAtPlayerPos(string category, int soundIndex)
        {
            if (!_audioConfig.AudioClips.TryGetValue(category, out var clips))
            {
                Debug.LogError("Audio clip for " + category + " does not exist");
                return;
            }
            AudioSource.PlayClipAtPoint(clips[soundIndex], transform.position, _audioSource.volume);
        }

        private List<string> GetCategories()
        {
            return new List<string>(_audioConfig.AudioClips.Keys);
        }
    }
}