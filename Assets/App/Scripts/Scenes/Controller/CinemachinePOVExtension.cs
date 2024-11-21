using Cinemachine;
using UnityEngine;

public class CinemachinePOVExtension : CinemachineExtension
{
	[SerializeField] private Controller controller;

	[SerializeField] private float horisontalSpeed = 10f;
	[SerializeField] private float verticalSpeed = 10f;
	[SerializeField] private float clamp = 80f;

	private Vector3 startRotation;

	protected override void Awake()
	{
		startRotation = transform.localRotation.eulerAngles;
		base.Awake();
	}

	protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
	{
		if (!vcam.Follow)
		{
			return;
		}
		if (stage == CinemachineCore.Stage.Aim)
		{
			var deltaInput = controller.GetMouseLook();

			startRotation.x += deltaInput.x * Time.fixedDeltaTime * horisontalSpeed;
			startRotation.y += deltaInput.y * Time.fixedDeltaTime * verticalSpeed;
			startRotation.y = Mathf.Clamp(startRotation.y, -clamp, clamp);

			state.RawOrientation = Quaternion.Euler(-startRotation.y, startRotation.x, 0f);
		}
	}
}
