using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Controller
{
	public interface IControllable
	{
		public void Move(Vector2 direction);

		public void Jump();

		public void StartAttack();
		public void CancelAttack();

		public void Reload();

	}
}
