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
using GameAnalyticsSDK;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace App.Scripts.Scenes.Gameplay.Player.Factories
{
    public class PlayerProvider
    {
        public event Action<Player> OnPlayerCreated;
        
        private readonly IGameInputProvider _gameInputProvider;
        private readonly PlayerController _playerController;
        private readonly InventoryProvider _inventoryProvider;
        private readonly ShootingModeFactory _shootingModeFactory;
        private readonly CameraProvider _cameraProvider;
        private readonly Player _playerPrefab;

        private List<Transform> _spawnPoints;
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

        public PlayerProvider(IGameInputProvider gameInputProvider,
            PlayerController playerController,
            Player playerPrefab,
            InventoryProvider inventoryProvider,
            ShootingModeFactory shootingModeFactory,
            CameraProvider cameraProvider)
        {
            _gameInputProvider = gameInputProvider;
            _playerController = playerController;
            _playerPrefab = playerPrefab;
            _inventoryProvider = inventoryProvider;
            _shootingModeFactory = shootingModeFactory;
            _cameraProvider = cameraProvider;
        }

        private Player Create()
        {
            var player = PhotonNetwork.Instantiate(
                _playerPrefab.gameObject.name,
                Vector3.zero,
                Quaternion.identity).GetComponent<Player>();
            player.Initialize(PhotonNetwork.NickName);
            var skin = _inventoryProvider.GameInventory.Skin;
            player.PlayerVisual.RPCSetSkin(skin);
            
            GameAnalytics.NewDesignEvent($"inventory:skin:{skin}",1 );
            GameAnalytics.NewDesignEvent($"inventory:weapons:{string.Join(",", _inventoryProvider.GameInventory.Weapons)}",1);
            foreach (var weapon in _inventoryProvider.GameInventory.Weapons)
            {
                GameAnalytics.NewDesignEvent($"inventory:weapon:{weapon}", 1);
            }
            
            _cameraProvider.RegisterCamera(player.VirtualCamera);
            
            player.WeaponProvider.Initialize(_gameInputProvider, _playerController, _inventoryProvider,_shootingModeFactory, player);
            OnPlayerCreated?.Invoke(player);
            return player;
        }

        public void RespawnPlayer()
        {
            Vector3 position = Vector3.one;
            if (_spawnPoints != null && _spawnPoints.Count > 0)
            {
                position = _spawnPoints[Random.Range(0, _spawnPoints.Count)].position;
            }
            
            _player.Teleport(position);
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

        public void HidePlayer()
        {
            _player.RPCSetActive(false);
        }

        public void SetSpawnPoints(List<Transform> spawnPoints)
        {
            _spawnPoints = spawnPoints;
        }
    }
}