using System;
using System.Collections.Generic;
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

        [Header("Walking")]
        [SerializeField] private float _stepInterval = 0.5f;
        [SerializeField, ValueDropdown(nameof(GetCategories))] private string _walkingCategory;

        [Header("Jumping")]
        [SerializeField, ValueDropdown(nameof(GetCategories))] private string _jumpingCategory;
        [SerializeField, ValueDropdown(nameof(GetCategories))] private string _landingCategory;
        [Header("Damage")]
        [SerializeField, ValueDropdown(nameof(GetCategories))] private string _damageCategory;

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
        
        public void PlayDamageSound()
        {
            RPCPlaySound(_damageCategory, Random.Range(0,_audioConfig.AudioClips[_damageCategory].Count));
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

        private List<string> GetCategories()
        {
            return new List<string>(_audioConfig.AudioClips.Keys);
        }
    }
}