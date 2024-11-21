using App.Scripts.Scenes;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class NetworkPlayer : NetworkBehaviour, IControllable, IHealth, IDamageable
{
	private const float GRAVITY = -9.81f;

	[Header("Movement")]
	[SerializeField] private float speed = 2.0f;
	[SerializeField] private float jumpHeight = 1.0f;
	[SerializeField] private float maxHealth;

	[SerializeField] private Transform _eyeView;

	[SerializeField] private Transform checkGroundPivot;
	[SerializeField] private LayerMask checkGroundMask;
	[SerializeField] private CinemachineVirtualCamera _virtualCamera;

	[field: SerializeField] public NetworkWeapon CurrentNetworkWeapon { get; private set; }

	private float velocity;
	private bool isGrounded;
	CharacterController controller;
	private Vector3 moveDirection;
	private Transform cameraTransform;

	public Health Health { get; private set; }

	public override void OnNetworkSpawn()
	{
		if (!IsClient || !IsOwner) return;
		Health = new(maxHealth);

		controller = GetComponent<CharacterController>();
		cameraTransform = _virtualCamera.transform;
		_virtualCamera.Priority = 99;
	}

	private void FixedUpdate()
	{
		isGrounded = isOnTheGround();
		if (isGrounded && velocity < 0)
		{
			velocity = -2f;
		}

		MoveInternal();
		DoGravity();
	}

	public void Attack()
	{
		if (!IsOwner || CurrentNetworkWeapon == null)
		{
			return;
		}
		
		CurrentNetworkWeapon.Attack();
	}

	public void Jump()
	{
		if (!IsOwner)
		{
			return;
		}
		
		if (isGrounded)
		{
			velocity = Mathf.Sqrt(jumpHeight * -2 * GRAVITY);
		}
	}

	public void Move(Vector2 direction)
	{
		if (!IsOwner)
		{
			return;
		}
		
		moveDirection =  new(direction.x,0,direction.y);
		moveDirection = cameraTransform.forward * moveDirection.z + cameraTransform.right * moveDirection.x;
		moveDirection.y = 0f;
	}

	private void MoveInternal()
	{
		controller.Move(moveDirection * Time.fixedDeltaTime * speed);
	}

	private bool isOnTheGround()
	{
		float checkGroundRadius = 0.4f;
		return Physics.CheckSphere(checkGroundPivot.position, checkGroundRadius, checkGroundMask);
	}

	private void DoGravity()
	{
		velocity += GRAVITY * Time.fixedDeltaTime;
		controller.Move(Vector3.up * velocity * Time.fixedDeltaTime);
	}

	public void TakeDamage(float damage)
	{
		Health.TakeDamage(damage);
	}
}
