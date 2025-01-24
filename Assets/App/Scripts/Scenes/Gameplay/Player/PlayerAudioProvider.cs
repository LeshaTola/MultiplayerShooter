using System;
using System.Collections.Generic;
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
        [SerializeField] private WeaponProvider _weaponProvider;

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

        private float _stepTimer;
        
        private void Update()
        {
            _stepTimer -= Time.deltaTime;
        }

        public void PlayWalkingSound()
        {
            if (_stepTimer > 0)
            {
                return;   
            }
            RPCPlaySound(_walkingCategory, Random.Range(0,_audioConfig.AudioClips[_walkingCategory].Count));
            _stepTimer = _stepInterval;
        }
        
        public void PlayJumpingSound()
        {
            RPCPlaySound(_jumpingCategory, Random.Range(0,_audioConfig.AudioClips[_jumpingCategory].Count));
        }
        
        public void PlayLandingSound()
        {
            RPCPlaySound(_landingCategory, Random.Range(0,_audioConfig.AudioClips[_landingCategory].Count));
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
                PlaySound(_damageCategory, Random.Range(0,_audioConfig.AudioClips[_damageCategory].Count));
            }
        }

        public void PlayHitSound()
        {
            PlaySound(_hitCategory, Random.Range(0,_audioConfig.AudioClips[_hitCategory].Count));
        }
        
        public void PlayKillSound()
        {
            PlaySound(_killCategory, Random.Range(0,_audioConfig.AudioClips[_killCategory].Count));
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
            _audioSource.PlayOneShot(_weaponProvider.CurrentWeapon.Config.ShotSound);
        }
        
        [PunRPC]
        public void PlayReloadSound()
        {
            _audioSource.PlayOneShot(_weaponProvider.CurrentWeapon.Config.ReloadSound);
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