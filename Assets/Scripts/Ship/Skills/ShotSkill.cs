
using UnityEngine;

public class ShotSkill : ShipSkill
{
	private readonly ShotSkillData _data;

	private bool _isUsing;

	// private float _baseDamage;

	public override bool CanUse => !_isUsing;

	public ShotSkill(Transform owner, ShotSkillData data) : base(owner)
	{
		_data = data;
	}

	// public void SetBaseDamage(float damage)
	// {
	// 	_baseDamage = damage;
	// }

	public override void Use(ShipAttack attack)
	{
		if (!CanUse)
			return;
		Debug.Log($"Using skill {_data.name}");

		attack.UseSkill(_data);
	}

	// private void Fire()
	// {
	// 	for (int i = 0; i < _data.Count; i++)
	// 	{
	// 		Projectile projectile = Object.Instantiate(
	// 			_data.ProjectilePrefab,
	// 			Owner.position,
	// 			Owner.rotation
	// 		);

	// 		float damage =
	// 			_baseDamage * _data.DamageMultiplier;

	// 		// projectile.Initialize(damage);
	// 	}
	// }
}