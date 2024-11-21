using System;

public class Health
{
	public event Action<float, float> OnValueChanged;
	public event Action OnDied;

	public float Value { get; private set; }
	public float MaxValue { get; private set; }

	public Health(float maxValue)
	{
		MaxValue = maxValue;
		Value = MaxValue;
	}

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

	public void Heal(float healValue)
	{
		if (healValue < 0)
		{
			return;
		}

		Value += healValue;
		if (Value > 0)
		{
			Value = healValue;
		}
		OnValueChanged?.Invoke(Value, MaxValue);
	}
}
