using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
	[SerializeField]
	private TMP_Text _indexText;

	[SerializeField]
	private Button _button;

	[SerializeField]
	private Image _icon;

	[SerializeField]
	private GameObject _groupCooldown;

	[SerializeField]
	private Image _progress;

	[SerializeField]
	private TMP_Text _timer;

	private SkillSlot _skillSlot;

	public void Initialize(SkillSlot skillSlot, int skillIndex)
	{
		_skillSlot = skillSlot;
		_indexText.text = (skillIndex + 1).ToString();
		// RefreshSkillSlot();

		if (_icon != null && _skillSlot != null && _skillSlot.Data != null && _skillSlot.Data.Icon != null)
			_icon.sprite = _skillSlot.Data.Icon;

		gameObject.SetActive(true);
	}

	private void Start()
	{
		_groupCooldown.SetActive(false);
	}

	private void OnEnable()
	{
		if (_button != null)
			_button.onClick.AddListener(UseSkill);
		if (_skillSlot != null)
			_skillSlot.OnCooldownChanged += UpdateCooldown;
	}

	private void OnDisable()
	{
		if (_button != null)
			_button.onClick.RemoveListener(UseSkill);

		if (_skillSlot != null)
			_skillSlot.OnCooldownChanged -= UpdateCooldown;
	}

	private void UseSkill()
	{
		_skillSlot?.Use();
	}

	private void UpdateCooldown(float timeRemaining)
	{
		float cooldownRatio = _skillSlot == null || _skillSlot.Data == null || _skillSlot.Data.Cooldown <= 0f ? 0f : timeRemaining / _skillSlot.Data.Cooldown;
		_groupCooldown.SetActive(cooldownRatio > 0f);

		if (_progress != null)
			_progress.fillAmount = cooldownRatio;

		if (_timer != null)
			_timer.text = timeRemaining > 0f ? Mathf.CeilToInt(timeRemaining).ToString() : string.Empty;

		var color = _icon.color;
		color.a = timeRemaining > 0f ? 15f / 255f : 1f;
		_icon.color = color;

		if (_button != null)
			_button.interactable = _skillSlot != null && _skillSlot.CanUse;
	}
}