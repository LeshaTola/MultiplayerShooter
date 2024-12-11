using System;
using App.Scripts.Scenes.Gameplay.Controller;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Stats
{
	public class Health : MonoBehaviourPun, IDamageable
	{
		public event Action<float, float> OnValueChanged;
		public event Action<float> OnDamage;
		public event Action<float> OnHealing;
		public event Action OnDied;

		[SerializeField] private float _maxHealth;

		public float Value { get; private set; }
		public float MaxValue { get; private set; }
		public int LastHitPlayerId { get; private set; }

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

		public void RPCSetLasHitPlayer(int playerId)
		{
			photonView.RPC(nameof(SetLasHitPlayer), RpcTarget.All, playerId);
		}
		
		[PunRPC]
		public void SetLasHitPlayer(int playerId)
		{
			LastHitPlayerId = playerId;
		} 

		[PunRPC]
		public void TakeDamage(float damage)
		{
			if (damage < 0)
			{
				return;
			}

			Value -= damage;
			OnDamage?.Invoke(damage);
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
			OnHealing?.Invoke(healValue);
			if (Value > _maxHealth)
			{
				Value = _maxHealth;
			}
			OnValueChanged?.Invoke(Value, MaxValue);
		}
	}
}
