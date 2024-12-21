using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Controller
{
	public interface IControllable
	{
		public void Move(Vector2 direction);

		public void MoveCamera(Vector2 offset);

		public void Jump();

		public void StartAttack();
		public void CancelAttack();
		
		public void StartAttackAlternative();
		public void CancelAttackAlternative();

		public void Reload();

	}
}
