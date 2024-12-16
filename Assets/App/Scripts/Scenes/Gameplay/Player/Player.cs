using App.Scripts.Features.Input;
using App.Scripts.Scenes.Gameplay.Controller;
using App.Scripts.Scenes.Gameplay.Player.Configs;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using App.Scripts.Scenes.Gameplay.Weapons;
using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Scenes.Gameplay.Player
{
	[RequireComponent(typeof(CharacterController))]
	public class Player : MonoBehaviourPun, IControllable
	{
		private const float GRAVITY = -9.81f;
		
		[SerializeField] private PlayerConfig _playerConfig;
		[Space]
		[SerializeField] private CharacterController _controller;
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
		
		private GameInputProvider _gameInputProvider;

		public string NickName { get; private set; }

		public PlayerConfig PlayerConfig => _playerConfig;

		public void Initialize(string name, GameInputProvider gameInputProvider)
		{
			_gameInputProvider = gameInputProvider;
			
			photonView.RPC(nameof(InitializePlayer), RpcTarget.AllBuffered, name);
			
			WeaponProvider.Initialize(_gameInputProvider,this);
			Health.Initialize(_playerConfig.MaxHealth);
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
			WeaponProvider.CurrentWeapon.StartAttack();
		}
		
		public void CancelAttack()
		{
			WeaponProvider.CurrentWeapon.CancelAttack();
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
			_controller.Move(_moveDirection * (Time.fixedDeltaTime * PlayerConfig.Speed));
		}

		private bool IsOnTheGround()
		{
			float checkGroundRadius = 0.4f;
			return Physics.CheckSphere(_checkGroundPivot.position, checkGroundRadius, _checkGroundMask);
		}

		private void DoGravity()
		{
			_velocity += GRAVITY * PlayerConfig.JumpFallSpeed * Time.fixedDeltaTime;
			_controller.Move(Vector3.up * (_velocity * Time.fixedDeltaTime));
		}
	}
}
