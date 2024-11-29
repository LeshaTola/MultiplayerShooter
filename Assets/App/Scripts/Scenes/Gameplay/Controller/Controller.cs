using App.Scripts.Features.Input;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace App.Scripts.Scenes.Gameplay.Controller
{
	public class Controller : MonoBehaviour
	{
		[SerializeField] private GameObject _controllableObject;

		private IControllable _controllable;
		private GameInputProvider _gameInputProvider;

		public void Initialize(GameInputProvider gameInputProvider)
		{
			_gameInputProvider = gameInputProvider;
			_controllable = _controllableObject.GetComponent<IControllable>();

			_gameInputProvider.OnLeftMouse += AttackPerformed;
			_gameInputProvider.OnSpace += JumpPerformed;
		}

		private void OnDisable()
		{
			_gameInputProvider.OnLeftMouse -= AttackPerformed;
			_gameInputProvider.OnSpace -= JumpPerformed;
		}

		private void JumpPerformed()
		{
			_controllable.Jump();
		}

		private void AttackPerformed()
		{
			_controllable.Attack();
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}

		private void Update()
		{
			_controllable.Move(_gameInputProvider.GetMovementNormalized());
		}
	}
}
