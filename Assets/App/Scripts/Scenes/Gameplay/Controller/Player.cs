using System;
using App.Scripts.Scenes.Gameplay.Weapons;
using Cinemachine;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Controller
{
	[RequireComponent(typeof(CharacterController))]
	public class Player : MonoBehaviour, IControllable
	{
		private const float GRAVITY = -9.81f;

		[Header("Movement")]
		[SerializeField] private float _speed = 2.0f;
		[SerializeField] private float _jumpHeight = 1.0f;
		[SerializeField] private float _jumpFallSpeed = 1.0f;

		[Space]
		[SerializeField] private CharacterController _controller;
		[SerializeField] private Transform _checkGroundPivot;
		[SerializeField] private LayerMask _checkGroundMask;

		[Header("Other")]
		[SerializeField] private CinemachineVirtualCamera _virtualCamera;
		[SerializeField] private WeaponProvider _weaponProvider;
		
		private float _velocity;
		private bool _isGrounded;
		private Vector3 _moveDirection;

		private void FixedUpdate()
		{
			_isGrounded = IsOnTheGround();
			if (_isGrounded && _velocity < 0)
			{
				_velocity = -2f;
			}

			MoveInternal();
			DoGravity();
		}

		public void StartAttack()
		{
			_weaponProvider.CurrentWeapon.StartAttack();
		}
		
		public void CancelAttack()
		{
			_weaponProvider.CurrentWeapon.CancelAttack();
		}

		public void Reload()
		{
			_weaponProvider.CurrentWeapon.Reload();
		}

		public void Jump()
		{

			if (_isGrounded)
			{
				_velocity = Mathf.Sqrt(_jumpHeight * _jumpFallSpeed * -2 * GRAVITY);
			}
		}

		public void Move(Vector2 direction)
		{
			_moveDirection =  new(direction.x,0,direction.y);
			_moveDirection 
				= _virtualCamera.transform.forward * _moveDirection.z 
				  + _virtualCamera.transform.right * _moveDirection.x;
			_moveDirection.y = 0f;
		}

		public void Teleport(Vector3 position)
		{
			var offset = position - transform.position ;
			_controller.Move(offset);
		}

		private void MoveInternal()
		{
			_controller.Move(_moveDirection * Time.fixedDeltaTime * _speed);
		}

		private bool IsOnTheGround()
		{
			float checkGroundRadius = 0.4f;
			return Physics.CheckSphere(_checkGroundPivot.position, checkGroundRadius, _checkGroundMask);
		}

		private void DoGravity()
		{
			_velocity += GRAVITY * _jumpFallSpeed * Time.fixedDeltaTime;
			_controller.Move(Vector3.up * _velocity * Time.fixedDeltaTime);
		}
	}
}
