// using UnityEngine;

// public abstract class ShipSkill : MonoBehaviour
// {
// 	[SerializeField] private string _skillId;

// 	public string SkillId => string.IsNullOrWhiteSpace(_skillId) ? GetType().Name : _skillId;

// 	public virtual void Tick()
// 	{
// 	}

// 	public abstract bool TryActivate();
// }

using UnityEngine;

public abstract class ShipSkill
{
	protected readonly Transform Owner;

	protected ShipSkill(Transform owner)
	{
		Owner = owner;
	}

	public abstract bool CanUse { get; }

	public abstract void Use(ShipAttack attack);
}