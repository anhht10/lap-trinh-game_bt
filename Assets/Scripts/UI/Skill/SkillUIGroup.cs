using System.Collections.Generic;
using UnityEngine;

public class SkillUIGroup : MonoBehaviour
{
	[SerializeField]
	private ShipSkillManager _skillManager;

	[SerializeField]
	private SkillUI _skillPrefab;

	[SerializeField]
	private Transform _content;

	private readonly List<SkillUI> _skillItems = new();
	private int _displayedSkillCount = -1;

	private void Start()
	{
		Ship localPlayer = LocalPlayerManager.Instance.LocalPlayer;

		if (localPlayer == null)
			return;

		if (localPlayer != null && localPlayer.SkillManager != null)
			_skillManager = localPlayer.SkillManager;

		if (_content == null)
			_content = transform;

		RebuildIfNeeded();
	}

	private void Update()
	{
		if (_skillManager == null)
		{
			Ship localPlayer = LocalPlayerManager.Instance.LocalPlayer;
			if (localPlayer != null && localPlayer.SkillManager != null)
				_skillManager = localPlayer.SkillManager;

			return;
		}
		RebuildIfNeeded();
	}

	private void RebuildIfNeeded()
	{
		if (_skillManager == null || _skillPrefab == null)
			return;

		if (_displayedSkillCount == _skillManager.SkillCount)
			return;

		foreach (SkillUI skillItem in _skillItems)
		{
			if (skillItem != null)
				Destroy(skillItem.gameObject);
		}

		_skillItems.Clear();

		for (int index = 0; index < _skillManager.SkillCount; index++)
		{
			SkillUI skillItem = Instantiate(_skillPrefab, _content);
			skillItem.gameObject.SetActive(false);
			SkillSlot skillSlot = _skillManager.GetSkillSlot(index);
			skillItem.Initialize(skillSlot, index);
			_skillItems.Add(skillItem);
		}

		_displayedSkillCount = _skillManager.SkillCount;
	}
}