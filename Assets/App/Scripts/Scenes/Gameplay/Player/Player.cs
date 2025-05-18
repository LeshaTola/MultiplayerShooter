using System;
using App.Scripts.Scenes.Gameplay.Controller;
using App.Scripts.Scenes.Gameplay.Player.Configs;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using App.Scripts.Scenes.Gameplay.Player.Teams.Configs;
using App.Scripts.Scenes.Gameplay.Weapons;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using YG;

namespace App.Scripts.Scenes.Gameplay.Player
{
    public enum PlayerState
    {
        Normal,
        Grappling
    }
    
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviourPun, IControllable
    {
        [SerializeField] private PlayerConfig _playerConfig;

        [field: Header("Other")] 

        [SerializeField] private NickNameUI _nickNameUI;

        [field: SerializeField] public PlayerMovement PlayerMovement { get; private set; }
        [field: SerializeField] public PlayerAudioProvider PlayerAudioProvider { get; private set; }
        [field: SerializeField] public WeaponProvider WeaponProvider { get; private set; }
        [field: SerializeField] public Health Health { get; private set; }
        [field: SerializeField] public PlayerVisual PlayerVisual { get; private set; }
        

        public string NickName { get; private set; }
        public TeamConfig Team { get; private set; }

        public PlayerConfig PlayerConfig => _playerConfig;

        public void Initialize(string name)
        {
            photonView.RPC(nameof(InitializePlayer), RpcTarget.AllBuffered, name);
            YG2.onFocusWindowGame += OnGamePaused;
        }
        
        private void OnGamePaused(bool isPaused)
        {
            if (photonView.IsMine)
            {
                RPCSetActive(isPaused);
            }
        }

        [PunRPC]
        public void InitializePlayer(string playerName)
        {
            NickName = playerName;
            _nickNameUI.Setup(NickName, Color.white);
            Health.Initialize(_playerConfig.MaxHealth);
            Health.OnDamage += OnDamage;
            WeaponProvider.OnWeaponChanged += OnWeaponChanged;
        }

        private void OnDestroy()
        {
            YG2.onFocusWindowGame -= OnGamePaused;
            Health.OnDamage -= OnDamage;
            WeaponProvider.OnWeaponChanged -= OnWeaponChanged;
        }

        public void Move(Vector2 direction)
        {
            PlayerMovement.Move(direction);
        }

        public void MoveCamera(Vector2 offset)
        {
            PlayerMovement.MoveCamera(offset);
        }

        public void Jump()
        {
            PlayerMovement.Jump();
        }

        public void StartAttack()
        {
            WeaponProvider.CurrentWeapon.StartAttack(false);
        }

        public void CancelAttack()
        {
            WeaponProvider.CurrentWeapon.CancelAttack(false);
        }

        public void StartAttackAlternative()
        {
            WeaponProvider.CurrentWeapon.StartAttack(true);
        }

        public void CancelAttackAlternative()
        {
            WeaponProvider.CurrentWeapon.CancelAttack(true);
        }

        public void Reload()
        {
            WeaponProvider.CurrentWeapon.Reload();
        }

        public void RPCSetActive(bool active)
        {
            photonView.RPC(nameof(SetActivePlayer), RpcTarget.All, active);
        }

        [PunRPC]
        public void SetActivePlayer(bool active)
        {
            gameObject.SetActive(active);
        }
        
        public void SetTeam(TeamConfig team)
        {
            Team = team;
            _nickNameUI.Setup(NickName, Team.Color);
        }
        
        private void OnWeaponChanged(Weapon obj)
        {
            PlayerAudioProvider.PlaySwitchWeaponSound();
        }

        private void OnDamage(float obj)
        {
            PlayerAudioProvider.PlayDamageSound();
        }
    }
}