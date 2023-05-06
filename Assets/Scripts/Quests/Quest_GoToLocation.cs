using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(Quest_GoToLocation))]
public class Quest_GoToLocation : Quest, IQuestTarget {
	public Vector2 DesiredLocation;
	public float ToleranceRadius;

	public Vector2 TargetLocation => DesiredLocation;
	public Color Color => Color.yellow;

	public override bool CheckIfComplete() => Vector2.Distance(_player.transform.position, DesiredLocation) < ToleranceRadius;

	public override void DoUpdate() {
		if(CheckIfComplete())
			Complete();
	}

	public override void GenerateRandom() {
		DesiredLocation = Helpers.RandomCircle(Vector2.zero, 500, GameplayManager.WorldSize);
		ToleranceRadius = 50f;

		_reward = Mathf.CeilToInt(DesiredLocation.magnitude / 20);

		_name = $"Head over to {DesiredLocation}";
		_description = $"We need some scans of the area near {DesiredLocation}. Head over there and we'll pay {_reward} $ for them.";
	}
}
