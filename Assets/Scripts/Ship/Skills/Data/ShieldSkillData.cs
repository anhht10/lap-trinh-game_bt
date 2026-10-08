

using UnityEngine;

[CreateAssetMenu(fileName = "ShieldSkillData", menuName = "Ship/Skills/Shield")]
public class ShieldSkillData : SkillData
{
	[Header("Shield")]

	[SerializeField]
	private Shield _shieldPrefab;




	[Header("Durability")]

	[Tooltip("Độ bền tối đa của khiên. Khi độ bền giảm xuống 0, khiên sẽ bị phá.")]
	[SerializeField]
	[Min(0f)]
	private float _maxDurability = 500f;

	[Tooltip("Lượng sát thương tối đa mà khiên có thể hấp thụ trong một lần nhận sát thương.")]
	[SerializeField]
	[Min(0f)]
	private float _maxAbsorption = 0f;


	[Header("Damage Reduction")]

	[Tooltip("Tỷ lệ giảm sát thương của khiên. 0 = không giảm, 1 = giảm 100%.")]
	[SerializeField]
	[Range(0f, 0.9f)]
	private float _damageReduction = 0.3f;


	[Header("Block")]

	[Tooltip("Xác suất khiên chặn hoàn toàn một đòn đánh. 0.25 = 25% cơ hội block.")]
	[SerializeField]
	[Range(0f, 0.85f)]
	private float _blockRate = 0f;


	[Header("Reflect")]

	[Tooltip("Tỷ lệ sát thương phản lại kẻ tấn công. 0.25 = phản lại 25% sát thương.")]
	[SerializeField]
	[Range(0f, 1f)]
	private float _reflectPercent = 0f;


	// [Header("Element Resistance")]

	// [Tooltip("Khả năng kháng sát thương nguyên tố của khiên.")]
	// [SerializeField]
	// private ElementResistance[] _elementResistances;


	[Header("Regeneration")]

	[Tooltip("Lượng độ bền khiên được hồi mỗi giây.")]
	[SerializeField]
	[Min(0f)]
	private float _regenRate = 0f;

	[Tooltip("Thời gian phải chờ sau khi khiên nhận sát thương trước khi bắt đầu hồi độ bền.")]
	[SerializeField]
	[Min(0f)]
	private float _regenDelay = 0f;


	[Header("Break")]

	[Tooltip("Prefab hiệu ứng được phát ra khi khiên bị phá.")]
	[SerializeField]
	private GameObject _breakEffect;


	public Shield ShieldPrefab => _shieldPrefab;

	public float MaxDurability => _maxDurability;

	public float MaxAbsorption => _maxAbsorption;

	public float DamageReduction => _damageReduction;

	public float BlockRate => _blockRate;

	public float ReflectPercent => _reflectPercent;

	// public ElementResistance[] ElementResistances => _elementResistances;

	public float RegenRate => _regenRate;

	public float RegenDelay => _regenDelay;

	public GameObject BreakEffect => _breakEffect;


	public override ShipSkill CreateSkill(Transform owner)
	{
		return new ShieldSkill(owner, this);
	}
}

// [System.Serializable]
// public struct ElementResistance
// {
//     [Tooltip("Loại sát thương nguyên tố mà khiên có khả năng kháng.")]
//     public ElementType element;

//     [Tooltip("Tỷ lệ giảm sát thương nguyên tố. 0.5 = giảm 50%.")]
//     [Range(0f, 1f)]
//     public float resistance;
// }

// Chỉ số					Ý nghĩa
// --------------------------------------------
// Duration				Khiên tồn tại bao lâu
// MaxDurability			Tổng độ bền tối đa của khiên
// MaxAbsorption			Lượng damage tối đa khiên được phép hấp thụ từ một hit
// DamageReduction			% damage được giảm
// BlockRate				% cơ hội chặn hoàn toàn một hit
// ReflectPercent			% damage phản lại attacker
// ElementResistance		% kháng từng loại nguyên tố
// RegenRate				Độ bền hồi mỗi giây
// RegenDelay				Thời gian chờ trước khi bắt đầu hồi
// BreakEffect				Effect khi khiên bị phá

// Một lưu ý về MaxAbsorption
// Mình muốn nhấn mạnh cái này vì nó dễ bị nhầm với MaxDurability.

// Ví dụ:

// MaxDurability = 500
// MaxAbsorption = 100

// Enemy đánh 300.

// Nếu bạn thiết kế MaxAbsorption là giới hạn damage một hit, thì:

// Incoming = 300
// Shield chỉ hấp thụ tối đa = 100

// Phần còn lại 200 phải được xử lý theo luật của game.

// Còn nếu ý của bạn là:

// "Khiên có tổng cộng 500 HP và mỗi hit đều trừ vào 500 đó"

// thì không cần MaxAbsorption.

// Trong trường hợp đó chỉ cần:

// MaxDurability
// CurrentDurability

// Mình cũng khuyên dùng [Tooltip] thay cho comment // đối với các biến mà bạn muốn thấy trực tiếp khi chỉnh trong Unity Inspector. Comment // phù hợp hơn để giải thích logic trong code.