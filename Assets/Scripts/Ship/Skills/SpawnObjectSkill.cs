
using UnityEngine;

public class SpawnObjectSkill : ShipSkill
{
	private readonly SpawnObjectSkillData _data;

	private bool _isUsing;

	// private float _baseDamage;

	public override bool CanUse => !_isUsing;

	private SpawnObject _spawnObject;

	public SpawnObjectSkill(Transform owner, SpawnObjectSkillData data) : base(owner)
	{
		_data = data;
	}

	// public void SetBaseDamage(float damage)
	// {
	// 	_baseDamage = damage;
	// }

	public override void Use(ShipAttack attack)
	{
		Vector3 p = new(3.47f, 1.06f, 4.03f);
		_spawnObject = Object.Instantiate(
			_data.ObjectPrefab,
			Owner.position + p,
			Owner.rotation
		);
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