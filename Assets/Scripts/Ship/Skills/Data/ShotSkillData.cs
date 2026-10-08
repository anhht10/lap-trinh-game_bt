using UnityEngine;

[CreateAssetMenu(fileName = "ShotSkillData", menuName = "Ship/Skills/Shot")]
public class ShotSkillData : SkillData
{
	[Header("Projectile")]
	[SerializeField]
	private Projectile _projectilePrefab;

	[SerializeField]
	[Min(0f)]
	private float _damageMultiplier = 1f;



	// [SerializeField]
	// [Min(0f)]
	// private float _duration = 8f;

	// [SerializeField]
	// [Min(1)]
	// private int _count = 1;

	public Projectile ProjectilePrefab => _projectilePrefab;
	public float DamageMultiplier => _damageMultiplier;

	public override ShipSkill CreateSkill(Transform owner)
	{
		return new ShotSkill(owner, this);
	}
}