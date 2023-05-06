using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : Controller {
	private List<Questline> _questlines = new();

	public event Action<Questline> OnQuestlineAdded;
	public event Action<Questline> OnQuestlineRemoved;
	public event Action<int, int> OnDebtChange;

	public int Debt {
		get => _debt;
		set {
			if(value <= 0)
				GameplayManager.Win();
			if(value != _debt)
				OnDebtChange?.Invoke(_debt, value);
			_debt = value;

		}
	}

	[SerializeField] private int _debt = 10000;

	protected override void Awake() {
		base.Awake();

		this.GetComponentInHeiarchy<C_Health>().OnDeath += _ => StartCoroutine(PlayerController_OnDeath());
	}

	private IEnumerator PlayerController_OnDeath() {
		GameplayManager.AllowInput = false;
		yield return GameplayManager.DeathOverlay.ShowOverlay();
	}

	void FixedUpdate() {
		if(!GameplayManager.AllowInput)
			return;

		if(!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButton(0))
			_ship.FireTowards(Camera.main.ScreenToWorldPoint(Input.mousePosition));

		if(Input.GetKey(KeyCode.X)) {
			_ship.MoveTowards(_ship.Position);
			return;
		}

		float forward = 0;
		float yaw = 0;

		if(Input.GetKey(KeyCode.W))
			forward += 1;
		if(Input.GetKey(KeyCode.S))
			forward -= 0.5f;

		if(Input.GetKey(KeyCode.D))
			yaw -= 1;
		if(Input.GetKey(KeyCode.A))
			yaw += 1;

		_ship.Rotate(yaw);
		_ship.Accelerate(_ship.transform.up * forward);

		foreach(var questline in _questlines.ToList())
			questline.DoUpdate();
	}

	public IReadOnlyList<Questline> Questlines => _questlines;
	public void AddQuestline(Questline questline) {
		_questlines.Add(questline);
		OnQuestlineAdded?.Invoke(questline);
		questline.Setup(this);
		questline.OnQuestComplete += (_, reward, newQuest) => Questline_OnQuestComplete(questline, reward, newQuest);
	}

	private void Questline_OnQuestComplete(Questline questline, int reward, Quest newQuest) {
		Debt -= reward;

		if(newQuest != null)
			return;

		_questlines.Remove(questline);
		OnQuestlineRemoved?.Invoke(questline);
	}
}
