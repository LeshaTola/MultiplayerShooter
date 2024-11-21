using UnityEngine;

public class Controller : MonoBehaviour
{
	[SerializeField] private GameObject controllableObject;

	private Input input;
	private IControllable controllable;

	private void OnEnable()
	{
		controllable = controllableObject.GetComponent<IControllable>();
		input = new Input();
		input.Character.Enable();

		input.Character.Attack.performed += AttackPerformed;
		input.Character.Jump.performed += JumpPerformed;

	}

	private void OnDisable()
	{
		input.Character.Disable();
		input.Character.Attack.performed -= AttackPerformed;
		input.Character.Jump.performed -= JumpPerformed;
	}

	public Vector2 GetMovementNormalized()
	{
		Vector2 inputVector = input.Character.Move.ReadValue<Vector2>();
		inputVector = inputVector.normalized;
		return inputVector;
	}

	public Vector2 GetMouseLook()
	{
		Vector2 mouseLook;

		mouseLook = input.Character.MouseLook.ReadValue<Vector2>();

		return mouseLook;
	}

	private void JumpPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		controllable.Jump();
	}

	private void AttackPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		controllable.Attack();
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update()
	{
		controllable.Move(GetMovementNormalized());
	}

}
