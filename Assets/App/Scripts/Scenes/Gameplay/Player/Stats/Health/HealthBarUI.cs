using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.Gameplay.Player.Stats
{
	public class HealthBarUI : MonoBehaviour
	{
		[SerializeField] private Slider _slider;
		[SerializeField] private TextMeshProUGUI _text;
		[SerializeField] private Health _health;

		public void Initialize(Health player)
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
			if (_health == null)
			{
				return;
			}
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
			if (_text != null)
			{
				_text.text = $"{health}";
			}
		}
	}
}
