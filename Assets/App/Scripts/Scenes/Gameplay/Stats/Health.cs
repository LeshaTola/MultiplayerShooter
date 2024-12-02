using System;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Stats
{
	public class Health : MonoBehaviourPun, IDamageable
	{
		public event Action<float, float> OnValueChanged;
		public event Action OnDied;

		[SerializeField] private float _maxHealth;

		public float Value { get; private set; }
		public float MaxValue { get; private set; }

		private void Start()
		{
			MaxValue = _maxHealth;
			NetworkTakeHeal(_maxHealth);
		}
		
		public void NetworkTakeDamage(float damage)
		{
			photonView.RPC(nameof(TakeDamage), RpcTarget.All, damage);
		}
		
		public void NetworkTakeHeal(float healValue)
		{
			photonView.RPC(nameof(Heal), RpcTarget.All, healValue);
		}

		[PunRPC]
		public void TakeDamage(float damage)
		{
			if (damage < 0)
			{
				return;
			}

			Value -= damage;
			if (Value <= 0)
			{
				Value = 0;
				OnDied?.Invoke();
			}
			OnValueChanged?.Invoke(Value, MaxValue);
		}

		[PunRPC]
		public void Heal(float healValue)
		{
			if (healValue < 0)
			{
				return;
			}

			Value += healValue;
			if (Value > _maxHealth)
			{
				Value = _maxHealth;
			}
			OnValueChanged?.Invoke(Value, MaxValue);
		}
	}
}
