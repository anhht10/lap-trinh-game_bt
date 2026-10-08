
using System;
using UnityEngine;
// [RequireComponent(typeof(ShipMovement))]
// [RequireComponent(typeof(ShipAttack))]
public class Ship : MonoBehaviour
{

	public ShipMovement Movement { get; private set; }
	public ShipAttack Attack { get; private set; }
	public Health Health { get; private set; }
	public Energy Energy { get; private set; }

	public ShipSkillManager SkillManager { get; private set; }

	private void Awake()
	{
		Movement = GetComponent<ShipMovement>();
		Attack = GetComponent<ShipAttack>();
		Health = GetComponent<Health>();
		Energy = GetComponent<Energy>();
		SkillManager = GetComponent<ShipSkillManager>();
	}

	private void Start()
	{
		LocalPlayerManager.Instance.SetLocalPlayer(this);
	}
}
