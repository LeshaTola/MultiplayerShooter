using System;
using System.Collections.Generic;
using App.Scripts.Features.Input;
using App.Scripts.Scenes.Gameplay.Controller;
using App.Scripts.Scenes.Gameplay.Hitmark;
using App.Scripts.Scenes.Gameplay.HitVisualProvider;
using App.Scripts.Scenes.Gameplay.KillHud;
using App.Scripts.Scenes.Gameplay.Stats;
using App.Scripts.Scenes.Gameplay.UI.LeaderBoard;
using App.Scripts.Scenes.Gameplay.Weapons;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Player = App.Scripts.Scenes.Gameplay.Controller.Player;
using Random = UnityEngine.Random;

namespace App.Scripts.Scenes.Gameplay
{
    public class RoomController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private List<Transform> _spawnPoints;

        [SerializeField] private Player _playerPrefab;
        [SerializeField] private HealthBarUI _healthBarUI;
        [SerializeField] private WeaponView _weaponView;
        [SerializeField] private KillChatView _killChatView;
        [SerializeField] private Controller.Controller _playerController;
        [SerializeField] private PostProcessingProvider _postProcessingProvider;
        [SerializeField] private HitService _hitService;

        private Player _player;
        private Health _health;
        private GameInputProvider _gameInputProvider;

        [Inject]
        private void Construct(GameInputProvider gameInputProvider)
        {
            _gameInputProvider = gameInputProvider;
            Initialize();
        }

        private void Initialize()
        {
            InitPlayer();
            InitWeapons();
            InitHealth();

            _playerController.Initialize(_gameInputProvider, _player);
        }

        private void InitHealth()
        {
            _health = _player.GetComponent<Health>();
            _healthBarUI.Init(_health);
            _health.OnDied += OnPlayerDeath;
            _health.OnHealing += OnHealing;
            _health.OnDamage += (value) => _postProcessingProvider.ApplyDamageEffect();
        }

        private void InitWeapons()
        {
            _hitService.Initialize();
            var weaponProvider = _player.GetComponent<WeaponProvider>();
            _weaponView.Initialize(weaponProvider);
            weaponProvider.Initialize(_gameInputProvider, _player);
            weaponProvider.OnPlayerHit += (value) => _hitService.Hit(value, weaponProvider.CurrentWeapon.Config.Damage);
        }

        private void InitPlayer()
        {
            _player = PhotonNetwork.Instantiate(
                _playerPrefab.gameObject.name,
                _spawnPoints[Random.Range(0, _spawnPoints.Count)].position,
                Quaternion.identity).GetComponent<Player>();
            _player.Initialize(PhotonNetwork.NickName);
        }

        private void OnHealing(float obj)
        {
            _postProcessingProvider.ApplyHealEffect();
        }
        
        private void OnPlayerDeath()
        {
            LeaderBoardProvider.Instance.AddDeath();
            
            UpdateKillChat();
            RespawnPlayer();
        }

        private void UpdateKillChat()
        {
            var lastHitPlayer = PhotonView.Find(_health.LastHitPlayerId).GetComponent<Player>();
            _killChatView.RPCSpawnKillElement(
                lastHitPlayer.NickName,
                _player.NickName);
        }

        private void RespawnPlayer()
        {
            _player.Teleport(_spawnPoints[Random.Range(0, _spawnPoints.Count)].position);
            _health.NetworkTakeHeal(_health.MaxValue);
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("MainMenu");
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
    }
}
