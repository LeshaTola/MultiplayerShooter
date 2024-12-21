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

        [Header("Other")]
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;

        [SerializeField] private NickNameUI _nickNameUI;

        [field: SerializeField] public WeaponProvider WeaponProvider { get; private set; }
        [field: SerializeField] public Health Health { get; private set; }

        private float _velocity;
        private bool _isGrounded;
        private Vector3 _moveDirection;
        private float _verticalRotation = 0f;

        private float _mouseXOffst;
        private Vector3 _externalForces;

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
        }

        private void FixedUpdate()
        {
            _isGrounded = IsOnTheGround();
            if (_isGrounded && _velocity < 0)
            {
                _velocity = -2f;
            }

            MoveInternal(_moveDirection);
            ApplyExternalForces();
            DoGravity();

            _virtualCamera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
            transform.Rotate(Vector3.up * _mouseXOffst);
            RPCSetVertical(_verticalRotation);
        }

        public void StartAttack()
        {
            WeaponProvider.CurrentWeapon.StartAttack();
        }

        public void CancelAttack()
        {
            WeaponProvider.CurrentWeapon.CancelAttack();
        }

        public void StartAttackAlternative()
        {
            // WeaponProvider.CurrentWeapon.StartAttackAlternative();
        }

        public void CancelAttackAlternative()
        {
            // WeaponProvider.CurrentWeapon.CancelAttackAlternative();
        }

        public void Reload()
        {
            WeaponProvider.CurrentWeapon.Reload();
        }

        public void Jump()
        {
            if (_isGrounded)
            {
                _velocity = Mathf.Sqrt(PlayerConfig.JumpHeight * PlayerConfig.JumpFallSpeed * -2 * GRAVITY);
            }
        }

        public void Move(Vector2 direction)
        {
            _moveDirection = new Vector3(direction.x, 0, direction.y);
            _moveDirection
                = transform.forward * _moveDirection.z
                  + transform.right * _moveDirection.x;
            _moveDirection.y = 0f;
        }

        public void MoveCamera(Vector2 offset)
        {
            _mouseXOffst = offset.x * Time.deltaTime;
            float mouseY = offset.y * Time.deltaTime;

            _verticalRotation -= mouseY;
            _verticalRotation = Mathf.Clamp(_verticalRotation, -90, 90);
        }

        public void AddForce(Vector3 force)
        {
            _velocity = 0;
            force.y = force.y * PlayerConfig.JumpFallSpeed * 2;
            _externalForces += force;
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
                _virtualCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
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
            _controller.Move(direction * (Time.fixedDeltaTime * PlayerConfig.Speed));
        }

        private bool IsOnTheGround()
        {
            float checkGroundRadius = 0.4f;
            return Physics.CheckSphere(_checkGroundPivot.position, checkGroundRadius, _checkGroundMask);
        }

        private void DoGravity()
        {
            _velocity += CalculateGravity();
            _controller.Move(Vector3.up * (_velocity * Time.fixedDeltaTime));
        }

        private float CalculateGravity()
        {
            return  GRAVITY * PlayerConfig.JumpFallSpeed * Time.fixedDeltaTime;
        }

        private void ApplyExternalForces()
        {
            if (_externalForces.magnitude > 0.01f)
            {
                _controller.Move(_externalForces * Time.fixedDeltaTime);
                _externalForces = Vector3.Lerp(_externalForces, Vector3.zero, Time.fixedDeltaTime * PlayerConfig.JumpFallSpeed);
            }
        }
    }
}