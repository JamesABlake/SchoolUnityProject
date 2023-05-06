using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(Quest_ReturnHome))]
public class Quest_ReturnHome : Quest, IQuestManual {

	[SerializeField] string _buttonText = "Continue";

	public override bool CheckIfComplete() => false;
	public void TryContribute(PlayerController player) => Complete();
	public string GetButtonText() => _buttonText;

	public override void GenerateRandom() {
		_name = "Return to the station";
		_description = "Return to the station so we can analyze the data from your mission.";
		_reward = 10;
	}
}
