using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAtack : MonoBehaviour
{
    EnemyStats stats;
    Transform target;
    StructureStats targetStats;

    void Start()
    {
        stats = GetComponent<EnemyStats>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }
    
    void UpdateTarget()
    {
        GameObject[] turrets = GameObject.FindGameObjectsWithTag(stats.Objetive_Tag);
        if (turrets.Length <= 0)
            turrets = GameObject.FindGameObjectsWithTag(stats.CoreTag);
        float MinDistance = Mathf.Infinity;
        float distanceToTurret;
        GameObject nearestTurret = null;
        foreach (GameObject turret in turrets)
        {
            distanceToTurret = Vector3.Distance(transform.position, turret.transform.position);
            if (distanceToTurret < MinDistance)
            {
                MinDistance = distanceToTurret;
                nearestTurret = turret;
            }
        }

        if (nearestTurret != null && MinDistance <= stats.range)
        {
            target = nearestTurret.transform;
            stats.canMove = false;
            stats.canAtack = true;
            targetStats = nearestTurret.GetComponent<StructureStats>();
        }
        else
        {
            target = null;
            stats.canAtack = false;
            stats.canMove = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!stats.canMove && stats.canAtack)
        {
            Utility.LookOnTarget(transform,(target.position - transform.position).normalized,stats.speed);
            if (stats.fireCountdown <= 0)
            {
                Shoot();
                stats.fireCountdown = 1f / stats.fireRate;
            }

            stats.fireCountdown -= Time.deltaTime;
        }
    }

    
    void Shoot()
    {
        GameObject Bullet_O = Instantiate(stats.bullet, stats.firePoint.position, stats.firePoint.rotation);
        EnemyBullet bullet_sc = Bullet_O.GetComponent<EnemyBullet>();
        if (bullet_sc != null)
            bullet_sc.Seek(target);
    }
}
