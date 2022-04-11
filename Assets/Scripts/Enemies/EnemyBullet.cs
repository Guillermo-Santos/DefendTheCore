using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
	private Transform target;

	public int damage = 15;
	public float speed = 10f;
	public float explosionRadius = 0f;
	public GameObject ImpactEffect;

	public void Seek(Transform _target)
	{
		target = _target;
	}

	// Update is called once per frame
	void Update()
	{
		if (target == null)
		{
			Destroy(gameObject);
			return;
		}

		Vector3 dir = target.position - transform.position;

		float distanceFPS = speed * Time.deltaTime;

		if (dir.magnitude <= distanceFPS)
		{
			HitTarget();
			return;
		}

		transform.Translate(dir.normalized * distanceFPS, Space.World);
		transform.LookAt(target);
	}

	void HitTarget()
	{
		GameObject effect = (GameObject)Instantiate(ImpactEffect, transform.position, transform.rotation);
		effect.transform.GetComponent<ParticleSystem>().Play();
		Destroy(effect, 5f);
		if (explosionRadius > 0f)
		{
			Explode();
		}
		else
		{
			Damage(target);
		}
		Destroy(gameObject);
	}
	void Damage(Transform enemy)
	{
		StructureStats e = enemy.GetComponent<StructureStats>();
		if (e != null)
		{
			e.TakeDamage(damage);
		}
	}

	void Explode()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
		foreach (Collider collider in colliders)
		{
			if (collider.tag == "StrategicTurret")
			{
				Damage(collider.transform);
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, explosionRadius);
	}
}
