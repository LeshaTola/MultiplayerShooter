using System.Collections.Generic;
using App.Scripts.Features.Input;
using App.Scripts.Scenes.Gameplay.HitVisualProvider;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using App.Scripts.Scenes.Gameplay.Timer;
using App.Scripts.Scenes.Gameplay.Weapons;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Player.Factories
{
    public class PlayerProvider
    {
        private readonly GameInputProvider _gameInputProvider;
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
                }
                return _player;
            }
        }

        public PlayerProvider(GameInputProvider gameInputProvider,
            List<Transform> spawnPoints,
            Player playerPrefab)
        {
            _gameInputProvider = gameInputProvider;
            _spawnPoints = spawnPoints;
            _playerPrefab = playerPrefab;
        }

        public void CreatePlayer()
        {
            _player = Create();
        }

        public void HidePlayer()
        {
            _player.RPCSetActive(false);
        }

        public void RespawnPlayer()
        {
            _player.RPCSetActive(true);
            _player.Teleport(_spawnPoints[Random.Range(0, _spawnPoints.Count)].position);
            _player.Health.RPCTakeHeal(_player.Health.MaxValue);
        }
        
        private Player Create()
        {
            var player = PhotonNetwork.Instantiate(
                _playerPrefab.gameObject.name,
                _spawnPoints[Random.Range(0, _spawnPoints.Count)].position,
                Quaternion.identity).GetComponent<Player>();
            
            player.Initialize(PhotonNetwork.NickName, _gameInputProvider);
            
            return player;
        }
    }
}