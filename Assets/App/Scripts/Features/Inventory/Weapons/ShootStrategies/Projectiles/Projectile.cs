using System;
using System.Collections.Generic;
using App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies;
using App.Scripts.Features.Inventory.Weapons.WeaponEffects;
using App.Scripts.Scenes.Gameplay.Effectors.Strategy;
using App.Scripts.Scenes.Gameplay.Player;
using App.Scripts.Scenes.Gameplay.Weapons;
using App.Scripts.Scenes.Gameplay.Weapons.Factories;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace App.Scripts.Features.Inventory.Weapons.ShootStrategies.Projectiles
{
	public class Projectile : SerializedMonoBehaviour
	{
		[SerializeField] private float _lifeTime = 5f;
		[SerializeField] private Rigidbody _rb;

		[Inject]
		private ShootingModeFactory _shootingModeFactory;
		
		private Action<Vector3, GameObject> _onColisionAction;

		public void Setup(Action<Vector3, GameObject> onColisionAction)
		{
			_onColisionAction = onColisionAction;
		}
		
		public void Shoot(Vector3 dir, float speed)
		{
			_rb.AddForce(dir * speed, ForceMode.Impulse);
			Destroy(gameObject, _lifeTime);
		}
		
		private void OnCollisionEnter(Collision other)
		{
			_onColisionAction?.Invoke(other.contacts[0].point, other.gameObject);
			Destroy(gameObject);
		}
	}
}
