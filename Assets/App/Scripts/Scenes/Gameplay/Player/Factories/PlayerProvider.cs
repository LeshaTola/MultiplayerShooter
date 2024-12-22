using System;
using System.Collections.Generic;
using App.Scripts.Features.Input;
using App.Scripts.Features.Inventory;
using App.Scripts.Scenes.Gameplay.Cameras;
using App.Scripts.Scenes.Gameplay.Weapons.Factories;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace App.Scripts.Scenes.Gameplay.Player.Factories
{
    public class PlayerProvider
    {
        private readonly GameInputProvider _gameInputProvider;
        private readonly InventoryProvider _inventoryProvider;
        private readonly ShootingModeFactory _shootingModeFactory;
        private readonly CameraProvider _cameraProvider;
        private readonly List<Transform> _spawnPoints;
        private readonly Player _playerPrefab;

        private Player _player;

        public CinemachineVirtualCamera VirtualCamera { get; private set; }

        public Player Player
        {
            get
            {
                if (_player == null)
                {
                    _player = Create();
                }

                return _player;
            }
        }

        public PlayerProvider(GameInputProvider gameInputProvider,
            List<Transform> spawnPoints,
            Player playerPrefab,
            InventoryProvider inventoryProvider,
            ShootingModeFactory shootingModeFactory,
            CameraProvider cameraProvider)
        {
            _gameInputProvider = gameInputProvider;
            _spawnPoints = spawnPoints;
            _playerPrefab = playerPrefab;
            _inventoryProvider = inventoryProvider;
            _shootingModeFactory = shootingModeFactory;
            _cameraProvider = cameraProvider;
        }

        public void HidePlayer()
        {
            _player.RPCSetActive(false);
        }

        public async void RespawnPlayer()
        {
            _player.RPCSetActive(true);
            _player.Health.RPCSetImmortal(true);
            _player.Teleport(_spawnPoints[Random.Range(0, _spawnPoints.Count)].position);
            _player.Health.RPCTakeHeal(_player.Health.MaxValue);
            
            await UniTask.Delay(TimeSpan.FromSeconds(5));
            _player.Health.RPCSetImmortal(false);
            
            _player.Health.SetImmortal(true);
        }

        private Player Create()
        {
            var player = PhotonNetwork.Instantiate(
                _playerPrefab.gameObject.name,
                _spawnPoints[Random.Range(0, _spawnPoints.Count)].position,
                Quaternion.identity).GetComponent<Player>();
            player.Initialize(PhotonNetwork.NickName);
            
            _cameraProvider.RegisterCamera(player.VirtualCamera);
            
            player.WeaponProvider.Initialize(_gameInputProvider, _inventoryProvider,_shootingModeFactory, player);
            return player;
        }
    }
}