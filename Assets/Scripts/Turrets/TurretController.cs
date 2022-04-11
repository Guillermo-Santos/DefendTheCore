using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    private Transform target;
    private EnemyStats targetEnemy;
    [HideInInspector]
    public StructureStats stats;

    [Header("Use Bullets (default)")]
    public float fireRate = 1f;
    private float fireCountdown = 1f;
    public GameObject bullet;

    [Header("Use Laser")]
    public bool useLaser = false;
    public int damageOverTime=30;
    public float SlowPct = .5f;

    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;

    [Header("Unity Setups")]
    public string Objetive_Tag = "Enemy";
    public Transform edge;
    public float edge_speed = 4f;
    public Transform fireLine;
    public Transform firePoint;
    public LayerMask targetMask;
    public LayerMask obstacleMask;


    private bool canSee;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<StructureStats>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        if (stats.isWorking)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(Objetive_Tag);
            float MinDistance = Mathf.Infinity;
            float distanceToEnemy;
            GameObject nearestEnemy = null;
            foreach (GameObject enemy in enemies)
            {
                distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < MinDistance)
                {
                    MinDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }

            if (nearestEnemy != null && MinDistance <= stats.range)
            {
                target = nearestEnemy.transform;
                targetEnemy = nearestEnemy.GetComponent<EnemyStats>();
            }
            else
            {
                target = null;
            }
        }else
        {
            if (target != null)
            {
                target = null;
            }
        }
    }


    void Shoot()
    {
        GameObject Bullet_O = (GameObject)Instantiate(bullet, firePoint.position, firePoint.rotation);
        Bullet bullet_sc = Bullet_O.GetComponent<Bullet>();
        if (bullet_sc != null)
            bullet_sc.Seek(target);
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            if (useLaser)
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    impactEffect.Stop();
                    impactLight.enabled = false;
                }
            }
            return;
        }

        Utility.LookOnTarget(edge, (target.position - edge.position).normalized, edge_speed);
        Tracking();
        //Fire
        if (canSee)
        {
            if (useLaser)
            {
                Laser();
            }
            else
            {
                if (fireCountdown <= 0)
                {
                    Shoot();
                    fireCountdown = 1f / fireRate;
                }

                fireCountdown -= Time.deltaTime;
            }
        }

    }

    void Tracking()
    {
        // fl = Fire line Direction 
        Vector3 fld = fireLine.TransformDirection(Vector3.forward);
        RaycastHit Obstaclehit; 
        RaycastHit enemyhit;

        if (Physics.Raycast(fireLine.position, fld * stats.range, out Obstaclehit, stats.range, obstacleMask))
        {
            if (Obstaclehit.collider!= null)
            {
                canSee = false;
                Debug.DrawRay(fireLine.position, fld * Obstaclehit.distance, Color.red);
            }
            else
            {
                canSee = false;
                Debug.DrawRay(fireLine.position, fld * Obstaclehit.distance, Color.red);
            }

        }else if (Physics.Raycast(fireLine.position, fld * stats.range, out enemyhit, stats.range, targetMask))
        {
            if (enemyhit.collider.CompareTag(Objetive_Tag))
            {
                canSee = true;
                Debug.DrawRay(fireLine.position, fld * enemyhit.distance, Color.green);
            }
            else
            {
                canSee = false;
                Debug.DrawRay(fireLine.position, fld * enemyhit.distance, Color.red);
            }

        }
        else
        {
            canSee = false;
            Debug.DrawRay(fireLine.position, fld * stats.range, Color.red);
        }

    }

    void Laser()
    {
        //Laser damage

        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        targetEnemy.Slow(SlowPct);
        //Visual
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled=true;
        }
        lineRenderer.SetPosition(0,firePoint.position);
        lineRenderer.SetPosition(1,target.position);

        Vector3 iEdir = firePoint.position - target.position;
        impactEffect.transform.position = target.position + iEdir.normalized * .3f;
        impactEffect.transform.rotation = Quaternion.LookRotation(iEdir);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, GetComponent<StructureStats>().range);
    }

}