using System;
using System.Collections.Generic;
using App.Scripts.Scenes.Gameplay.Effectors.Strategy;
using App.Scripts.Scenes.Gameplay.Player;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using App.Scripts.Scenes.Gameplay.Weapons;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies.ShootStrategies.Projectiles
{
	public class Projectile : SerializedMonoBehaviour
	{
		[SerializeField] private float _lifeTime = 5f;
		[SerializeField] private List<EffectsWithDelay> _effects;
		[SerializeField] private Rigidbody _rb;
		
		private Weapon _weapon;

		public void Setup(Weapon weapon)
		{
			_weapon = weapon;
		}
		
		public void Shoot(Vector3 dir, float speed)
		{
			_rb.AddForce(dir * speed, ForceMode.Impulse);
			Destroy(gameObject, _lifeTime);
		}
		
		private async void OnCollisionEnter(Collision other)
		{
			/*if (_weapon.photonView.IsMine)
			{
				_weapon.SpawnImpact(other.contacts[0].point);
			}*/ //TODO: TO KEY POOL
			
			Destroy(gameObject);
			if (!other.gameObject.TryGetComponent(out Player player))
			{
				return;
			}
			
			foreach (var effect in _effects)
			{
				await effect.Effect.Apply(player);
				await UniTask.Delay(TimeSpan.FromSeconds(effect.Delay));
			}
		}
	}

	[Serializable]
	public class EffectsWithDelay
	{
		[SerializeReference]
		public IEffectorStrategy Effect;
		public float Delay;
	}
}
