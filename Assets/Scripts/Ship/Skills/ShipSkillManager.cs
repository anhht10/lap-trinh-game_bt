using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ShipInputHandler))]

public class ShipSkillManager : MonoBehaviour
{
	private ShipInputHandler _input;
	private Ship _ship;

	[SerializeField]
	private List<SkillSlot> _skills = new();

	public int SkillCount => _skills.Count;


	private void OnEnable()
	{
		if (_input != null)
			_input.OnSkillEvent += OnSkill;
	}

	private void OnDisable()
	{
		if (_input != null)
			_input.OnSkillEvent -= OnSkill;
	}
	private void Awake()
	{
		_input = GetComponent<ShipInputHandler>();
		_ship = GetComponent<Ship>();

		foreach (SkillSlot skill in _skills)
		{
			skill.Initialize(transform, _ship);
		}
	}

	private void Update()
	{
		foreach (SkillSlot skill in _skills)
		{
			skill.Update(Time.deltaTime);
		}
	}

	private void OnSkill(int index)
	{
		UseSkill(index - 1);
	}

	public void UseSkill(int index)
	{
		if (index < 0 || index >= _skills.Count)
		{
			Debug.LogWarning(
				$"Invalid skill index: {index}"
			);

			return;
		}

		_skills[index].Use();
	}

	public bool CanUseSkill(int index)
	{
		if (index < 0 || index >= _skills.Count)
			return false;

		return _skills[index].CanUse;
	}

	public SkillSlot GetSkillSlot(int index)
	{
		if (index < 0 || index >= _skills.Count)
			return null;

		return _skills[index];
	}
}