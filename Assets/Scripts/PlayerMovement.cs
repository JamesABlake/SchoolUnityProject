using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public Vector2 DesiredMovement {
		get => _desiredMovement;
		set => _desiredMovement = value;
	}
	public float DesiredYaw {
		get => -_desiredYaw;
		set => _desiredYaw = -value;
	}

	private Vector2 _desiredMovement;
	private float _desiredYaw;

	Rigidbody2D rigid;
	List<Quest> quests = new();
	public void AddQuest(Quest quest) => quests.Add(quest);

	private void Start() {
		rigid = GetComponent<Rigidbody2D>();
	}
	public void Update() {
		float forward = 0;
		float right = 0;
		float yaw = 0;

		if(Input.GetKey(KeyCode.W))
			forward += 1;
		if(Input.GetKey(KeyCode.S))
			forward -= 1;

		if(Input.GetKey(KeyCode.D))
			right += 1;
		if(Input.GetKey(KeyCode.A))
			right -= 1;

		if(Input.GetKey(KeyCode.E))
			yaw -= 1;
		if(Input.GetKey(KeyCode.Q))
			yaw += 1;

		if(Input.GetKey(KeyCode.X)) {
			forward -= Mathf.Clamp((Quaternion.Inverse(transform.rotation) * rigid.velocity).y, -1, 1);
			right -= Mathf.Clamp((Quaternion.Inverse(transform.rotation) * rigid.velocity).x, -1, 1);
			yaw -= Mathf.Clamp(rigid.angularVelocity, -1, 1);
		}

		if(right != 0 || forward != 0) {
			_desiredMovement += new Vector2(right, forward) * Time.deltaTime;
		}
		if(yaw != 0)
			_desiredYaw += yaw * Time.deltaTime;
	}
}
