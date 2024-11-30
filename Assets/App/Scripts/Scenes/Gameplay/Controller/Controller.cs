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

			_gameInputProvider.OnLeftMouseStarted += AttackStarted;
			_gameInputProvider.OnLeftMouseCanceled += AttackCanceled;
			_gameInputProvider.OnSpace += JumpPerformed;
			_gameInputProvider.OnR += ReloadPerformed;
		}

		private void OnDisable()
		{
			if (_gameInputProvider == null)
			{
				return;
			}
			_gameInputProvider.OnLeftMouseStarted -= AttackStarted;
			_gameInputProvider.OnLeftMouseCanceled -= AttackCanceled;
			_gameInputProvider.OnSpace -= JumpPerformed;
			_gameInputProvider.OnR -= ReloadPerformed;
		}

		private void JumpPerformed()
		{
			_controllable.Jump();
		}

		private void AttackStarted()
		{
			_controllable.StartAttack();
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}

		private void AttackCanceled()
		{
			_controllable.CancelAttack();
		}

		private void Update()
		{
			_controllable.Move(_gameInputProvider.GetMovementNormalized());
		}

		private void ReloadPerformed()
		{
			_controllable.Reload();
		}
	}
}
