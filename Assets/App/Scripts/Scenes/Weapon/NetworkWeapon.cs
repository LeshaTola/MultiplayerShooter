using DG.Tweening;
using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class NetworkWeapon : MonoBehaviour
{
	public event Action<int, int> OnAmmoChanged;

	[SerializeField] private int maxAmmoCount;

	[SerializeField] private float reloadCooldown;
	[SerializeField] private float attackCooldown;

	[SerializeField] private Bullet bulletTemplate;
	[SerializeField] private Transform bulletSpawnPoint;

	private int currentAmmoCount;

	public bool IsReady { get; private set; } = true;

	private void Awake()
	{
		currentAmmoCount = maxAmmoCount;
	}

	public void Attack()
	{

		if (!IsReady)
		{
			return;
		}
		if (currentAmmoCount <= 0)
		{
			Reload();
			return;
		}

		SpawnBulletServerRpc();

		AttackAnimation();

		currentAmmoCount--;
		OnAmmoChanged?.Invoke(currentAmmoCount, maxAmmoCount);
		StartAttackCooldown();
	}

	
	
	[ServerRpc]
	public void SpawnBulletServerRpc()
	{
		var bullet = Instantiate(bulletTemplate);
		bullet.GetComponent<NetworkObject>().Spawn(true);
		bullet.transform.position = bulletSpawnPoint.position;
		bullet.Shoot(transform.forward);
	}

	public void Reload()
	{
		ReloadAnimation();
		StartReloadCooldown();
		currentAmmoCount = maxAmmoCount;
		OnAmmoChanged?.Invoke(currentAmmoCount, maxAmmoCount);
	}

	private void AttackAnimation()
	{
		Sequence sequence = DOTween.Sequence();

		sequence.Append(transform.DOLocalRotate(new Vector3(-15, 0, 0), attackCooldown / 2));
		sequence.Append(transform.DOLocalRotate(new Vector3(0, 0, 0), attackCooldown / 2));
	}

	private void ReloadAnimation()
	{
		transform.DOLocalRotate(new Vector3(360, 0, 0), reloadCooldown, RotateMode.FastBeyond360);
	}

	private void StartAttackCooldown()
	{
		StartCoroutine(AttackCooldown());
	}

	private IEnumerator AttackCooldown()
	{
		IsReady = false;
		yield return new WaitForSeconds(attackCooldown);
		IsReady = true;
	}

	private void StartReloadCooldown()
	{
		StartCoroutine(ReloadCooldown());
	}

	private IEnumerator ReloadCooldown()
	{
		IsReady = false;
		yield return new WaitForSeconds(reloadCooldown);
		IsReady = true;
	}
}
