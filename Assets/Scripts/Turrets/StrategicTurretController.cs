using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategicTurretController : MonoBehaviour
{
    //Important variables
    private Transform target;
    private StructureStats targetStats;
    [HideInInspector]
    public StructureStats stats;

    [Header("Lase effects")]
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;

    [Header("Unity Setups")]
    public List<string> Objetive_Tag;
    public Transform edge;
    public float edge_speed = 4f;
    public Transform fireLine;
    public Transform firePoint;
    public LayerMask targetMask;
    public LayerMask obstacleMask;


    private bool canSee;
    private bool isProducing;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        stats = GetComponent<StructureStats>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    GameObject[] posibleTargets()
    {
        List<GameObject> pTargets = new List<GameObject>();
        foreach(string tag in Objetive_Tag)
        {
            GameObject[] found = GameObject.FindGameObjectsWithTag(tag);
            if(found != null && found.Length > 0)
                foreach(GameObject target in found)
                    if(target != null)
                        pTargets.Add(target);
        }
        return pTargets.ToArray();
    }

    void UpdateTarget()
    {
        if (stats.isWorking)
        {
            GameObject[] targets = posibleTargets();
            float MinDistance = Mathf.Infinity;
            float distanceToTarget;
            GameObject nearestTarget = null;
            foreach (GameObject target in targets)
            {
                //this condition prevent the healer turret to select it selft
                if (!this.gameObject.Equals(target))
                {
                    distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                    if (stats.isHealer)
                    {
                        StructureStats structure;
                        if (target.TryGetComponent<StructureStats>(out structure)) 
                        {
                            if ((distanceToTarget < MinDistance) && (structure.health < structure.maxHealth))
                            {
                                MinDistance = distanceToTarget;
                                nearestTarget = target;
                            } 
                        }
                    }
                    else if (distanceToTarget < MinDistance)
                    {
                        MinDistance = distanceToTarget;
                        nearestTarget = target;
                    }
                }
            }

            if (nearestTarget != null && MinDistance <= stats.range)
            {
                target = nearestTarget.transform;
                canSee = true;
            }
            else
            {
                target = null; 
                canSee = false;
            }   
        }
        else { 
            if (target != null) 
            {
                target = null;
                canSee = false;
                StopProduction();
            } 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            if (lineRenderer.enabled)
            {
                lineRenderer.enabled = false;
                impactEffect.Stop();
                impactLight.enabled = false;
            }
            return;
        }
        //Tracking target
        Utility.LookOnTarget(edge, (target.position - edge.position).normalized, edge_speed);
        //Tracking();
        //Fire
        if (canSee)
        {
            Laser();
            Produce();
        }else
        {
            StopProduction();
        }
    }

    void Laser()
    {
        if(!audioSource.isPlaying)
            audioSource.Play();
        //Visual
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 iEdir = firePoint.position - target.position;
        impactEffect.transform.position = target.position + iEdir.normalized * .3f;
        impactEffect.transform.rotation = Quaternion.LookRotation(iEdir);
    }

    void Produce()
    {
        if (!isProducing)
        {
            foreach (Product product in stats.products)
            {
                if (product.productionType == Production.Energy)
                {
                    PlayerStats.EnGeneration += product.production;
                    isProducing = true;
                }
                else if (product.productionType == Production.Material)
                {
                    PlayerStats.MatGeneration += product.production;
                    isProducing = true;
                }
                else if (product.productionType == Production.Health)
                {
                    targetStats = target.GetComponent<StructureStats>();
                    if (targetStats.health < targetStats.maxHealth)
                        targetStats.heal(product.production * Time.deltaTime);
                }
            }
        }
    }

    void StopProduction()
    {
        if (isProducing)
        {
            foreach (Product product in stats.products)
            {
                if (product.productionType == Production.Energy)
                {
                    PlayerStats.EnGeneration -= product.production;
                }
                else if (product.productionType == Production.Material)
                {
                    PlayerStats.MatGeneration -= product.production;
                }
            }
            isProducing = false;
        }
    }

    private void OnDestroy()
    {
        StopProduction();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, GetComponent<StructureStats>().range);
    }
}
