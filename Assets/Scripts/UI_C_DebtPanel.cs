using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_C_DebtPanel : MonoBehaviour {
	public TMP_Text Text;
	public Image Panel;
	public Color DebtDecreaseColor = Color.green;
	public Color DebtIncreaseColor = Color.red;

	private Color _originalColor;

	private void Awake() {
		var player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
		_originalColor = Panel.color;
		Text.text = $"{-player.Debt} $";
		player.OnDebtChange += Player_OnDebtChange;
	}

	private void Player_OnDebtChange(int oldValue, int newValue) {
		Text.text = $"{-newValue} $";
		if(newValue < oldValue)
			StartCoroutine(Helpers.FlashColor(Panel, DebtDecreaseColor, _originalColor));
		else
			StartCoroutine(Helpers.FlashColor(Panel, DebtIncreaseColor, _originalColor));
	}
}
