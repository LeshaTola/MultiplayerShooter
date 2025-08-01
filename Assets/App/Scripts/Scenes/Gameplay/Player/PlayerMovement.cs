﻿using App.Scripts.Scenes.Gameplay.Player.Configs;
using Cinemachine;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Player
{
    public interface IEntityMovement
    {
        void AddForce(Vector3 force);
        PlayerState PlayerState { get; set; }
        void Freese();
        void GaranteedMove(Vector3 delta);
    }

    public class PlayerMovement : MonoBehaviourPun, IEntityMovement
    {
        private const float Gravity = -9.81f;

        [SerializeField] private PlayerConfig _playerConfig;

        [Space]
        [SerializeField] private Transform _checkGroundPivot;

        [SerializeField] private LayerMask _checkGroundMask;

        [field: Space] [field: SerializeField] public CharacterController Controller { get; private set; }
        [field: SerializeField] public CinemachineVirtualCamera VirtualCamera { get; private set; }
        [field: SerializeField] public PlayerAudioProvider PlayerAudioProvider { get; private set; }
        [field: SerializeField] public PlayerVisual PlayerVisual { get; private set; }

        private float _velocity;
        private Vector3 _moveDirection;
        private Vector3 _moveVelocityIdk;
        private Vector3 _moveVelocity;
        private Vector3 _currentMoveVelocity;

        private float _verticalRotation;
        private float _targetHorizontalOffset;

        public bool IsGrounded { get; private set; }
        public PlayerState PlayerState { get; set; } = PlayerState.Normal;

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

            IsGrounded = IsLanded();
            if (IsGrounded && _velocity < 0)
            {
                _velocity = -5f;
            }
        }

        private void LateUpdate()
        {
            MoveCamera();
        }

        public void Move(Vector2 direction)
        {
            Vector3 move = transform.forward * direction.y + transform.right * direction.x;
            move.y = 0;

            _moveDirection = move;

            PlayerVisual.MoveAnimation(move);
        }

        public void MoveCamera(Vector2 offset)
        {
            _targetHorizontalOffset = offset.x;
            float mouseY = offset.y;

            _verticalRotation -= mouseY;
            _verticalRotation = Mathf.Clamp(_verticalRotation, -90, 90);
        }

        public void Jump()
        {
            if (IsGrounded)
            {
                JumpInternal(_playerConfig.JumpHeight);
                PlayerAudioProvider.PlayJumpingSound();
            }
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

        public void Freese()
        {
            _moveDirection = Vector3.zero;
            _moveVelocity = Vector3.zero;
            _velocity = 0f;
        }

        public void GaranteedMove(Vector3 delta)
        {
            Controller.Move(delta);
        }

        public void Teleport(Vector3 position)
        {
            Controller.enabled = false;
            transform.position = position;
            Controller.enabled = true;
        }

        private void JumpInternal(float height)
        {
            _velocity = Mathf.Sqrt(height * _playerConfig.JumpFallSpeed * -2 * Gravity);
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
            return Gravity * _playerConfig.JumpFallSpeed * Time.deltaTime;
        }

        private void MoveInternal(Vector3 direction)
        {
            if (_moveVelocity.magnitude > _playerConfig.Speed)
            {
                var dampingForce = direction * _playerConfig.Speed;
                var nextVelocity = _moveVelocity + dampingForce * Time.deltaTime;
                nextVelocity -= nextVelocity * Time.deltaTime;
                if (nextVelocity.magnitude < _moveVelocity.magnitude)
                {
                    _moveVelocity = nextVelocity;
                }

                if (_moveVelocity.magnitude <= _playerConfig.Speed)
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
                    _moveVelocity + direction * _playerConfig.Speed,
                    ref _moveVelocityIdk, 0.1f);

            Controller.Move(Time.deltaTime * _currentMoveVelocity);

            PlayWalkingSound();
        }

        private void MoveCamera()
        {
            VirtualCamera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
            transform.Rotate(Vector3.up * _targetHorizontalOffset);
        }

        private bool IsLanded()
        {
            var isGroundedNow = IsOnTheGround();
            if (isGroundedNow && !IsGrounded && _velocity < Gravity)
            {
                PlayerVisual.LandAnimation();
                PlayerAudioProvider.PlayLandingSound();
            }

            return isGroundedNow;
        }

        private void PlayWalkingSound()
        {
            if (!IsGrounded || _currentMoveVelocity.magnitude < 0.2f)
            {
                return;
            }

            PlayerAudioProvider.PlayWalkingSound();
        }
    }
}