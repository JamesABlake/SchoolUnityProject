using System;
using UnityEngine;

public class C_Health : MonoBehaviour {
	/// <summary>0.0 to 1.0</summary>
	public float HealthPercent => _health / _maxHealth;
	public float MaxHealth => _maxHealth;
	public float Health => _health;

	public event Action<GameObject> OnDeath;
	public event Action<GameObject> OnTakeDamage;
	public bool IsAlive { get; private set; } = true;

	[SerializeField]
	private float _maxHealth = 100;
	[SerializeField]
	private float _health = 100;

	public void SetMaxHealth(float maxHealth) => _maxHealth = maxHealth;
	public void SetHealth(float health) => _health = health;

	public void TakeDamage(float damage, GameObject src) {
		if(!IsAlive)
			return;

		OnTakeDamage?.Invoke(src);

		_health = Mathf.Clamp(_health - damage, 0, _maxHealth);
		if(_health <= 0) {
			OnDeath?.Invoke(src);
			IsAlive = false;
		}
	}
}
