using System;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Player.Stats
{
	public class Health : MonoBehaviourPun, IDamageable
	{
		public event Action<float, float> OnValueChanged;
		public event Action<float> OnDamage;
		public event Action<float> OnHealing;
		public event Action OnDied;

		[SerializeField] private int _startHealth = -1;
		
		[field: SerializeField] public bool IsPlayer { get; private set; } = false;
		
		public float Value { get; private set; }
		public float MaxValue { get; private set; }
		public int LastHitPlayerId { get; private set; }
		public string LastHitWeaponId { get; private set; }
		public bool IsImortal { get; private set; }

		public void Start()
		{
			if (_startHealth <= 0)
			{
				return;
			}
			Initialize(_startHealth);
		}

		public void Initialize(int _maxHealth)
		{
			MaxValue = _maxHealth;
			RPCTakeHeal(_maxHealth);
		}
		
		public void RPCSetImmortal(bool isImortal)
		{
			photonView.RPC(nameof(SetImmortal), RpcTarget.All, isImortal);
		}

		[PunRPC]
		public void SetImmortal(bool isImortal)
		{
			IsImortal = isImortal;
		}
		
		public void RPCTakeDamage(float damage)
		{
			photonView.RPC(nameof(TakeDamage), RpcTarget.All, damage);
		}
		
		public void RPCTakeHeal(float healValue)
		{
			photonView.RPC(nameof(Heal), RpcTarget.All, healValue);
		}

		public void RPCSetLasHit(int playerId, string weaponId)
		{
			photonView.RPC(nameof(SetLasHit), RpcTarget.All, playerId, weaponId);
		}
		
		[PunRPC]
		public void SetLasHit(int playerId, string weaponId)
		{
			LastHitPlayerId = playerId;
			LastHitWeaponId = weaponId;
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
			if (Value > MaxValue)
			{
				Value = MaxValue;
			}
			OnValueChanged?.Invoke(Value, MaxValue);
		}
	}
}
