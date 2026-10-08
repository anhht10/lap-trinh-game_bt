using UnityEngine;

public abstract class SkillData : ScriptableObject
{

	// Thêm type skill。
	[SerializeField]
	private Sprite _icon;

	[SerializeField]
	[Min(0f)]
	private float _cooldown = 10f;

	[SerializeField]
	private string _description;

	public Sprite Icon => _icon;
	public float Cooldown => _cooldown;

	[Header("Cost")]
	[SerializeField]
	[Min(0f)]
	private float _energyCost = 0f;

	public float EnergyCost => _energyCost;

	[Header("Skill Type")]
	[SerializeField]
	private bool _isTimeSkill;

	[Tooltip("Thời gian khiên tồn tại sau khi được kích hoạt, tính bằng giây./=0 = vô hạn.")]
	[SerializeField]
	[Min(0f)]
	private float _duration = 10f;

	[SerializeField]
	[Min(1)]
	private int _count = 1;

	public float Duration => _duration;

	public int Count => _count;

	public bool IsTimeSkill => _isTimeSkill;
	/// <summary>
	/// Tạo runtime skill tương ứng với Data này.
	/// </summary>
	public abstract ShipSkill CreateSkill(Transform owner);
}
