using System.Collections;
using UnityEngine;
[RequireComponent(typeof(ShipInputHandler))]
[RequireComponent(typeof(ShipMovement))]

[RequireComponent(typeof(Ship))]
public class ShipAttack : MonoBehaviour
{
	[SerializeField] private float _attack = 10f;

	private ShipInputHandler _input;
	private Blaster[] _blasters;
	private ShipMovement _shipMovement;
	private Ship _ship;
	private bool _isAiming;

	public bool IsAiming => _isAiming;

	private void Awake()
	{
		_input = GetComponent<ShipInputHandler>();
		_blasters = GetComponentsInChildren<Blaster>();
		_shipMovement = GetComponent<ShipMovement>();
		_ship = GetComponent<Ship>();
	}

	private void Update()
	{
		if (_input.AttackHold)
		{
			if (_ship.Energy == null || _ship.Energy.IsEmpty)
				return;
			Fire();
		}
	}

	public bool Fire()
	{
		// float damageMultiplier = _data == null ? 1f : _data.DamageMultiplier;
		return FireProjectile(null, 1);
	}

	public void UseSkill(ShotSkillData data)
	{
		foreach (Blaster blaster in _blasters)
			blaster.SetSkillData(data);
	}

	public bool FireProjectile(Projectile projectileOverride, float damageMultiplier = 1f)
	{
		if (_isAiming)
			return false;

		StartCoroutine(FireAfterRotation(projectileOverride, damageMultiplier));
		return true;
	}

	private IEnumerator FireAfterRotation(Projectile projectileOverride, float damageMultiplier)
	{
		_isAiming = true;
		if (_input.TPCameraTogger)
			_shipMovement.RotateTowardsShoot();

		yield return new WaitForFixedUpdate();

		foreach (Blaster blaster in _blasters)
		{
			if (blaster == null)
				continue;
			if (blaster.CanFire)
			{
				bool tryConsumeEnergy = _ship.Energy.TryConsume(0.5f);
				if (tryConsumeEnergy)
					blaster.TryFire(_ship, _attack, damageMultiplier);
			}
		}

		_isAiming = false;
	}

}