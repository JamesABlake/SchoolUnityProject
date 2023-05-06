using System;
using UnityEngine;

public abstract class Quest : ScriptableObject {
	public virtual string Name => _name;
	public virtual string Description => _description;
	public virtual int Reward => _reward;

	[SerializeField] protected string _name;
	[SerializeField] protected string _description;
	[SerializeField] protected int _reward;

	public event Action OnChanged;
	public event Action<int> OnComplete;

	protected PlayerController _player;

	public abstract bool CheckIfComplete();
	protected void Changed() {
		OnChanged?.Invoke();
		if(CheckIfComplete())
			Complete();
	}
	protected void Complete() => OnComplete?.Invoke(Reward);

	public virtual void Setup(PlayerController player) {
		_player = player;
	}
	public virtual void DoUpdate() { }
	public abstract void GenerateRandom();
}