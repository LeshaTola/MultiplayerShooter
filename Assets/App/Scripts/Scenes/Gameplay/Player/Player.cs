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
    public enum PlayerState
    {
        Normal,
        Grappling
    }
    
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviourPun, IControllable
    {
        private const float GRAVITY = -9.81f;

        [SerializeField] private PlayerConfig _playerConfig;

        [field: Space] [field: SerializeField] public CharacterController Controller { get; private set; }

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
        public PlayerState PlayerState { get; set; } = PlayerState.Normal;

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
            switch (PlayerState)
            {
                case PlayerState.Normal:
                    MoveInternal(_moveDirection);
                    DoGravity();

                    break;
                case PlayerState.Grappling:
                    break;
            }
            
            _isGrounded = IsLanded();
            if (_isGrounded && _velocity < 0)
            {
                _velocity = -5f;
            }
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
        
        public void JumpToTarget(Vector3 targetPoint, float trajectoryHeight)
        {
            /*if (!_isGrounded)
                PlayerAudioProvider.PlayJumpingSound();
                */
            Vector3 jumpVelocity = CalculateJumpVelocity(transform.position, targetPoint, trajectoryHeight);
            
            _velocity = jumpVelocity.y;
            _moveDirection = 
            _moveVelocity = new Vector3(jumpVelocity.x, 0, jumpVelocity.z);

        }

        public void Freese()
        {
            _moveDirection = Vector3.zero;
            _moveVelocity = Vector3.zero;
            _velocity = 0f;
        }

        public void Move(Vector2 direction)
        {

            _moveDirection = new (direction.x, 0, direction.y);
            PlayerVisual.MoveAnimation(_moveDirection);
            
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
            Controller.enabled = false;
            transform.position = position;
            Controller.enabled = true;
        }

        private void MoveInternal(Vector3 direction)
        {
            if (_moveVelocity.magnitude > PlayerConfig.Speed)
            {
                var dampingForce = direction * PlayerConfig.Speed;
                var nextVelocity = _moveVelocity + dampingForce * Time.deltaTime;
                 nextVelocity -= nextVelocity * Time.deltaTime;
                if (nextVelocity.magnitude < _moveVelocity.magnitude)
                {
                    _moveVelocity = nextVelocity;
                }
                if (_moveVelocity.magnitude <= PlayerConfig.Speed)
                {
                    _moveVelocity = Vector3.zero;
                }
            }
            else
            {
                _moveVelocity = Vector3.zero;
            }
            
            _currentMoveVelocity 
                = Vector3.SmoothDamp(_currentMoveVelocity, 
                    _moveVelocity + direction * PlayerConfig.Speed,
                    ref _moveVelocityIdk, 0.1f);
            
            Controller.Move(Time.deltaTime * _currentMoveVelocity);
            
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
                PlayerVisual.LandAnimation();
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
            PlayerVisual.JumpAnimation();
        }

        private bool IsOnTheGround()
        {
            float checkGroundRadius = 0.4f;
            return Physics.CheckSphere(_checkGroundPivot.position, checkGroundRadius, _checkGroundMask);
        }

        private void DoGravity()
        {
            _velocity += CalculateGravity();
            Controller.Move(Vector3.up * (_velocity * Time.deltaTime));
        }

        private float CalculateGravity()
        {
            return GRAVITY * PlayerConfig.JumpFallSpeed * Time.deltaTime;
        }
        
        private Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
        {
            float gravity = GRAVITY * PlayerConfig.JumpFallSpeed;
            float displacementY = endPoint.y - startPoint.y;
            Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

            Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
    
            float timeUp = Mathf.Sqrt(-2 * trajectoryHeight / gravity);
            float timeDown = Mathf.Sqrt(2 * Mathf.Max(0, displacementY - trajectoryHeight) / -gravity);
            float totalTime = timeUp + timeDown;

            Vector3 velocityXZ = displacementXZ / totalTime;

            return velocityXZ + velocityY;
        }

    }
}