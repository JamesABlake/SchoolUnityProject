using UnityEngine;

public class C_Thruster : MonoBehaviour {
	ParticleSystem _particleSystem;
	Rigidbody2D _rigidbody;
	Spaceship _spaceship;
	private void Start() {
		_particleSystem = GetComponent<ParticleSystem>();
		_rigidbody = this.GetComponentInHeiarchy<Rigidbody2D>();
		_spaceship = this.GetComponentInHeiarchy<Spaceship>();
	}

	// Update is called once per frame
	void Update() {
		var emitter = _particleSystem.main;
		emitter.emitterVelocity = _rigidbody.velocity;
		var emission = _particleSystem.emission;
		var amount = Vector2.Dot(transform.up, _spaceship.Acceleration);
		if(amount > 0.1f)
			emission.rateOverTime = Mathf.Clamp01(amount) * 100f;
		else
			emission.rateOverTime = 0;
	}
}
