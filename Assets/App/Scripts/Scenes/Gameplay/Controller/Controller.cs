using App.Scripts.Features.Input;
using App.Scripts.Scenes.Gameplay.UI.LeaderBoard;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace App.Scripts.Scenes.Gameplay.Controller
{
	public class Controller : MonoBehaviour
	{
		[SerializeField] private LeaderBoard _leaderBoard;

		private IControllable _controllable;
		private GameInputProvider _gameInputProvider;

		public void Initialize(GameInputProvider gameInputProvider, IControllable controllable)
		{
			_gameInputProvider = gameInputProvider;
			_controllable = controllable;

			_gameInputProvider.OnLeftMouseStarted += AttackStarted;
			_gameInputProvider.OnLeftMouseCanceled += AttackCanceled;
			_gameInputProvider.OnSpace += JumpPerformed;
			_gameInputProvider.OnR += ReloadPerformed;
			_gameInputProvider.OnTabPerformed += OnTabPerformed;
			_gameInputProvider.OnTabCanceled += OnTabCanceled;
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
			_controllable.MoveCamera(_gameInputProvider.GetMouseLook());
		}

		private void ReloadPerformed()
		{
			_controllable.Reload();
		}

		private void OnTabPerformed()
		{
			_leaderBoard.Show();
		}

		private void OnTabCanceled()
		{
			_leaderBoard.Hide();
		}
	}
}
