using App.Scripts.Scenes.Gameplay.Player.Stats;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Weapons.ShootStrategies.Projectiles
{
	public class Bullet : MonoBehaviour
	{
		[SerializeField] private float damage;
		[SerializeField] private float lifeTime;
		[SerializeField] private float speed;

		private Rigidbody rb;

		private void Awake()
		{
			rb = GetComponent<Rigidbody>();
		}

		public void Shoot(Vector3 dir)
		{
			rb.AddForce(dir * speed, ForceMode.Impulse);
			Destroy(gameObject, lifeTime);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.TryGetComponent(out IDamageable damageable))
			{
				damageable.TakeDamage(damage);
			}
			Destroy(gameObject);
		}

	}
}
