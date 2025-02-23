using System;
using App.Scripts.Scenes.Gameplay.Controller;
using App.Scripts.Scenes.Gameplay.Player.Configs;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using App.Scripts.Scenes.Gameplay.Weapons;
using Cinemachine;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviourPun, IControllable
    {
        private const float GRAVITY = -9.81f;

        [SerializeField] private PlayerConfig _playerConfig;

        [Space]
        [SerializeField] private CharacterController _controller;

        [Space]
        [SerializeField] private Transform _checkGroundPivot;

        [SerializeField] private LayerMask _checkGroundMask;

        [field: Header("Other")] [field: SerializeField]
        public CinemachineVirtualCamera VirtualCamera { get; private set; }

        [SerializeField] private NickNameUI _nickNameUI;
        
        [field: SerializeField] public PlayerAudioProvider PlayerAudioProvider { get; private set; }
        [field: SerializeField] public WeaponProvider WeaponProvider { get; private set; }
        [field: SerializeField] public Health Health { get; private set; }
        [field: SerializeField] public PlayerVisual PlayerVisual { get; private set; }

        private float _velocity;
        private Vector3 _moveDirection;
        private Vector3 _moveVelocityIdk;
        private Vector3 _moveVelocity;
        private Vector3 _currentMoveVelocity;
        private bool _isGrounded;

        private float _verticalRotation;
        private float _targetHorizontalOffset;

        public string NickName { get; private set; }

        public PlayerConfig PlayerConfig => _playerConfig;

        public void Initialize(string name)
        {
            photonView.RPC(nameof(InitializePlayer), RpcTarget.AllBuffered, name);
        }

        [PunRPC]
        public void InitializePlayer(string playerName)
        {
            NickName = playerName;
            _nickNameUI.Setup(NickName);
            Health.Initialize(_playerConfig.MaxHealth);
            Health.OnDamage += OnDamage;
        }

        private void OnDamage(float obj)
        {
            PlayerAudioProvider.PlayDamageSound();
        }

        private void Update()
        {
            _isGrounded = IsLanded();
            if (_isGrounded && _velocity < 0)
            {
                _velocity = -5f;
            }
            
            MoveInternal(_moveDirection);
            DoGravity();
            MoveCamera();
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

        public void Jump()
        {
            if (_isGrounded)
            {
                JumpInternal(PlayerConfig.JumpHeight);
                PlayerAudioProvider.PlayJumpingSound();
            }
        }

        public void Move(Vector2 direction)
        {
            _moveDirection = new (direction.x, 0, direction.y);
            _moveDirection
                = transform.forward * _moveDirection.z
                  + transform.right * _moveDirection.x;
            _moveDirection.y = 0f;
        }

        public void MoveCamera(Vector2 offset)
        {
            _targetHorizontalOffset = offset.x;
            float mouseY = offset.y;

            _verticalRotation -= mouseY;
            _verticalRotation = Mathf.Clamp(_verticalRotation, -90, 90);
        }

        public void AddForce(Vector3 force)
        {
            if (force.y > 0)
            {
                JumpInternal(force.y);
            }
            else
            {
                _velocity += force.y;
            }
            force.y = 0;

            _moveVelocity += force;
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

        private void RPCSetVertical(float verticalRotation)
        {
            photonView.RPC(nameof(SetVertical), RpcTarget.All, verticalRotation);
        }

        [PunRPC]
        public void SetVertical(float verticalRotation)
        {
            if (!photonView.IsMine)
            {
                VirtualCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
            }
        }

        public void Teleport(Vector3 position)
        {
            _controller.enabled = false;
            transform.position = position;
            _controller.enabled = true;
        }

        private void MoveInternal(Vector3 direction)
        {
            if (_moveVelocity.magnitude > PlayerConfig.Speed)
            {
                var dampingForce = direction * PlayerConfig.Speed;
                var nextVelocity = _moveVelocity + dampingForce * Time.deltaTime;
                if (nextVelocity.magnitude < _moveVelocity.magnitude)
                {
                    _moveVelocity = nextVelocity;
                }
                
                _moveVelocity = Vector3.Lerp(_moveVelocity, Vector3.zero, Time.deltaTime);
            }
            else
            {
                _moveVelocity = Vector3.zero;
            }
            
            _currentMoveVelocity 
                = Vector3.SmoothDamp(_currentMoveVelocity, 
                    _moveVelocity + direction * PlayerConfig.Speed,
                    ref _moveVelocityIdk, 0.1f);
            
            _controller.Move(Time.deltaTime * _currentMoveVelocity);
            
            PlayWalkingSound();
        }

        private void MoveCamera()
        {
            VirtualCamera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
            transform.Rotate(Vector3.up * _targetHorizontalOffset );
            RPCSetVertical(_verticalRotation);
        }

        private bool IsLanded()
        {
            var isGroundedNow = IsOnTheGround();
            if (isGroundedNow && !_isGrounded && _velocity < GRAVITY)
            {
                PlayerAudioProvider.PlayLandingSound();
            }

            return isGroundedNow;
        }

        private void PlayWalkingSound()
        {
            if (!_isGrounded || _currentMoveVelocity.magnitude < 0.2f)
            {
                return;
            }
            PlayerAudioProvider.PlayWalkingSound();
        }

        private void JumpInternal(float height)
        {
            _velocity = Mathf.Sqrt( height * PlayerConfig.JumpFallSpeed * -2 * GRAVITY);
        }

        private bool IsOnTheGround()
        {
            float checkGroundRadius = 0.4f;
            return Physics.CheckSphere(_checkGroundPivot.position, checkGroundRadius, _checkGroundMask);
        }

        private void DoGravity()
        {
            _velocity += CalculateGravity();
            _controller.Move(Vector3.up * (_velocity * Time.deltaTime));
        }

        private float CalculateGravity()
        {
            return GRAVITY * PlayerConfig.JumpFallSpeed * Time.deltaTime;
        }
    }
}