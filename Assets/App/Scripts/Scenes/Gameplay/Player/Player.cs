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

		[SerializeField] private PlayerInputConfig _playerInputConfig;
		[Space]
		[SerializeField] private CharacterController _controller;
		[SerializeField] private Transform _checkGroundPivot;
		[SerializeField] private LayerMask _checkGroundMask;

		[Header("Other")]
		[SerializeField] private CinemachineVirtualCamera _virtualCamera;
		[SerializeField] private WeaponProvider _weaponProvider;
		[SerializeField] private NickNameUI _nickNameUI;
		
		private float _velocity;
		private bool _isGrounded;
		private Vector3 _moveDirection;
		private float _verticalRotation = 0f;

		public string NickName { get; private set; }

		public PlayerInputConfig PlayerInputConfig => _playerInputConfig;

		public void Initialize(string name)
		{
			photonView.RPC(nameof(InitializePlayer), RpcTarget.AllBuffered, name);
		}
		
		[PunRPC]
		public void InitializePlayer(string playerName)
		{
			NickName = playerName;
			_nickNameUI.Setup(NickName);
		}
		
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
				_velocity = Mathf.Sqrt(PlayerInputConfig.JumpHeight * PlayerInputConfig.JumpFallSpeed * -2 * GRAVITY);
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

		public void MoveCamera(Vector2 offset)
		{
			float mouseX = offset.x * Time.deltaTime;
			float mouseY = offset.y * Time.deltaTime;

			_verticalRotation -= mouseY;
			_verticalRotation = Mathf.Clamp(_verticalRotation, -80, 80);
			
			RPCSetVertical(_verticalRotation);
			transform.Rotate(Vector3.up * mouseX);
		}

		private void RPCSetVertical(float verticalRotation)
		{
			photonView.RPC(nameof(SetVertical),RpcTarget.All,verticalRotation);
		}
		
		[PunRPC]
		public void SetVertical(float verticalRotation)
		{
			_virtualCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
		}

		public void Teleport(Vector3 position)
		{
			_controller.enabled = false;
			transform.position = position;
			_controller.enabled = true;
		}

		private void MoveInternal()
		{
			_controller.Move(_moveDirection * (Time.fixedDeltaTime * PlayerInputConfig.Speed));
		}

		private bool IsOnTheGround()
		{
			float checkGroundRadius = 0.4f;
			return Physics.CheckSphere(_checkGroundPivot.position, checkGroundRadius, _checkGroundMask);
		}

		private void DoGravity()
		{
			_velocity += GRAVITY * PlayerInputConfig.JumpFallSpeed * Time.fixedDeltaTime;
			_controller.Move(Vector3.up * (_velocity * Time.fixedDeltaTime));
		}
	}
}
