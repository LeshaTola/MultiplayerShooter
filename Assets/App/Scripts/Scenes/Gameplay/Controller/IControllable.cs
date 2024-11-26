using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Controller
{
	public interface IControllable
	{
		public void Move(Vector2 direction);

		public void Jump();

		public void Attack();

	}
}
