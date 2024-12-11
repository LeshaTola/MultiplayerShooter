using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.Gameplay.Player.Stats
{
	public class HealthBarUI : MonoBehaviour
	{
		[SerializeField] private Slider _slider;
		[SerializeField] private Health _health;

		public void Init(Health player)
		{
			_health = player;
			_health.OnValueChanged += OnHealthChanged;
		}
		
		private void OnEnable()
		{
			if(_health == null)
			{
				return;
			}
			
			_health.OnValueChanged += OnHealthChanged;
		}

		private void OnDisable()
		{
			_health.OnValueChanged -= OnHealthChanged;
		}

		private void OnHealthChanged(float health, float maxValue)
		{
			UpdateUI(health, maxValue);
		}

		private void UpdateUI(float health, float maxHealth)
		{
			_slider.maxValue = maxHealth;
			_slider.value = health;
		}
	}
}
