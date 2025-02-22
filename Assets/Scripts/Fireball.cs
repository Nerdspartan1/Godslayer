﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
	public GameObject Target;
	public Vector2 InitialSpeed = new Vector2(4f, 4f);
	public Vector2 HomingAcceleration = new Vector2(1, 2);
	public float Damage = 1f;

	public float LifeTime = 8f;

	private Rigidbody _rb;
	private float _acceleration;

    FMOD.Studio.EventInstance fireballEvent;

    void Start()
    {
		_rb = GetComponent<Rigidbody>();
		_rb.velocity = Random.Range(InitialSpeed.x, InitialSpeed.y) * transform.forward;
		_acceleration = Random.Range(HomingAcceleration.x, HomingAcceleration.y);

        fireballEvent = FMODUnity.RuntimeManager.CreateInstance(SoundManager.sm.skelmagefireballtravel);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(fireballEvent, this.transform, this.GetComponent<Rigidbody>());
        fireballEvent.start();
	}

    void Update()
    {
		_rb.AddForce((Target.transform.position - transform.position).normalized * _acceleration, ForceMode.Acceleration);
		LifeTime -= Time.deltaTime;
        if (LifeTime < 0)
        {
            Destroy(gameObject);
            fireballEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            fireballEvent.release();
        }
        
        fireballEvent.setParameterByName("Fireball Travel", Vector3.Distance(Target.transform.position, this.gameObject.transform.position));
        //Debug.Log(Vector3.Distance(Target.transform.position, this.gameObject.transform.position));
    }

	private void OnCollisionEnter(Collision collision)
	{
		Destroy(gameObject);

        fireballEvent.setParameterByName("Fireball Travel", 0f);
        fireballEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        fireballEvent.release();
        FMODUnity.RuntimeManager.PlayOneShotAttached(SoundManager.sm.fireballimpact, this.gameObject);


        var player = collision.gameObject.GetComponent<Player>();
		if (player)
		{
            var impactPoint = new GameObject();
			impactPoint.transform.position = transform.position;
			Destroy(impactPoint, 2f);
			player.Damage(new vp_DamageInfo(Damage, impactPoint.transform, vp_DamageInfo.DamageType.Bullet));
		}
	}
}
