using System;
using System.Collections.Generic;
using App.Scripts.Features.Input;
using App.Scripts.Features.Inventory;
using App.Scripts.Scenes.Gameplay.Cameras;
using App.Scripts.Scenes.Gameplay.Controller;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using App.Scripts.Scenes.Gameplay.Weapons;
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
        private readonly PlayerController _playerController;
        private readonly InventoryProvider _inventoryProvider;
        private readonly ShootingModeFactory _shootingModeFactory;
        private readonly CameraProvider _cameraProvider;
        private readonly List<Transform> _spawnPoints;
        private readonly Player _playerPrefab;

        private Player _player;

        public Player Player
        {
            get
            {
                if (_player == null)
                {
                    _player = Create();
                    HidePlayer();
                }

                return _player;
            }
        }

        public PlayerProvider(GameInputProvider gameInputProvider,
            PlayerController playerController,
            List<Transform> spawnPoints,
            Player playerPrefab,
            InventoryProvider inventoryProvider,
            ShootingModeFactory shootingModeFactory,
            CameraProvider cameraProvider)
        {
            _gameInputProvider = gameInputProvider;
            _playerController = playerController;
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

        public void RespawnPlayer()
        {
            _player.Teleport(_spawnPoints[Random.Range(0, _spawnPoints.Count)].position);
            _player.RPCSetActive(true);
            
            _player.Health.RPCTakeHeal(_player.Health.MaxValue);
            _player.Health.RPCSetLasHit(_player.photonView.ViewID, null);

            foreach (var weapon in _player.WeaponProvider.Weapons)
            {
                if (!weapon)
                {
                    return;
                }
                
                weapon.ReloadImmidiate();
            }
        }

        private Player Create()
        {
            var player = PhotonNetwork.Instantiate(
                _playerPrefab.gameObject.name,
                _spawnPoints[Random.Range(0, _spawnPoints.Count)].position,
                Quaternion.identity).GetComponent<Player>();
            player.Initialize(PhotonNetwork.NickName);
            player.PlayerVisual.RPCSetSkin(_inventoryProvider.GameInventory.Skin);
            
            _cameraProvider.RegisterCamera(player.VirtualCamera);
            
            player.WeaponProvider.Initialize(_gameInputProvider, _playerController, _inventoryProvider,_shootingModeFactory, player);
            return player;
        }
    }
}