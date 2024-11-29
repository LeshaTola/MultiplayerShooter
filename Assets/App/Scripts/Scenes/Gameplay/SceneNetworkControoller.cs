using System;
using System.Collections.Generic;
using App.Scripts.Features.Input;
using App.Scripts.Scenes.Gameplay.Controller;
using App.Scripts.Scenes.Gameplay.Stats;
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
            _player = PhotonNetwork.Instantiate(
                _playerPrefab.gameObject.name,
                _spawnPoints[Random.Range(0, _spawnPoints.Count)].position,
                Quaternion.identity).GetComponent<Player>();
            _health = _player.GetComponent<Health>();
            _player.GetComponent<Controller.Controller>().Initialize(_gameInputProvider);
            _player.GetComponent<WeaponProvider>().Initialize(_gameInputProvider);
            _healthBarUI.Init(_health);
            _health.OnDied += RespawnPlayer;
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
