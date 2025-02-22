﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCaster : Enemy
{
	public float CastingCooldown = 10f;
	public float CastRange;
	public GameObject CastProjectile;
	public int NumberOfProjectiles;
	protected float _timeBeforeNextCast = 0f;
	protected float MinimumCastCooldown = 1f;

	protected override void Start()
	{
		base.Start();
		_timeBeforeNextCast = Random.Range(0f, CastingCooldown);
	}

	protected override void Update()
	{
		_anim.SetFloat("speed", _nav.velocity.magnitude);
		if (Target) _nav.SetDestination(Target.transform.position);

		float distance = Vector3.Distance(Target.transform.position, HitCast.transform.position);

		if ((_timeBeforeNextCast < 0f && distance < CastRange) || (distance < Range && _timeBeforeNextCast < CastingCooldown - MinimumCastCooldown))
		{
			_nav.isStopped = true;
			_anim.SetTrigger("cast");
			_timeBeforeNextCast = CastingCooldown;
		}

		_timeBeforeNextCast -= Time.deltaTime;

	}

	public virtual void Cast()
	{
		
		for(int i = 0; i < NumberOfProjectiles; ++i)
		{
			var proj = Instantiate(CastProjectile, HitCast.transform.position, Quaternion.LookRotation(0.5f * Vector3.up +  Random.onUnitSphere) , GameManager.Instance.Game.transform).GetComponent<Fireball>();
			proj.Target = Target.FPSCamera.gameObject;
		}
		if(_nav) _nav.isStopped = false;
	}
}
