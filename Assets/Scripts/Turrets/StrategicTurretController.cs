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


    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<StructureStats>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    GameObject[] posibleTargets()
    {
        List<GameObject> pTargets = new List<GameObject>();
        foreach(string tag in Objetive_Tag)
        {
            GameObject[] found = GameObject.FindGameObjectsWithTag(tag);
            pTargets.AddRange(found);
        }
        return pTargets.ToArray();
    }

    void UpdateTarget()
    {
        if (stats.isWorking)
        {
            GameObject[] enemies = posibleTargets();
            float MinDistance = Mathf.Infinity;
            float distanceToTarget;
            GameObject nearestTarget = null;
            foreach (GameObject target in enemies)
            {
                //this condition prevent the healer turret to select it selft
                if (!this.gameObject.Equals(target))
                {
                    distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                    if (stats.isHealer)
                    {
                        StructureStats structure = target.GetComponent<StructureStats>();
                        if ((distanceToTarget < MinDistance) && structure.health < structure.maxHealth)
                        {
                            MinDistance = distanceToTarget;
                            nearestTarget = target;
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
            }
            else
            {
                target = null;
            }
        }
        else { 
            if (target != null) 
            {
                target = null;
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
        Tracking();
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

    bool compareTagsWith(Collider collider)
    {
        foreach(string tag in Objetive_Tag)
        {
            if(collider.CompareTag(tag))
                return true;
        }
        return false;
    }

    void Tracking()
    {
        // fl = Fire line Direction 
        Vector3 fld = fireLine.TransformDirection(Vector3.forward);
        RaycastHit Obstaclehit;
        RaycastHit enemyhit;

        if (Physics.Raycast(fireLine.position, fld * stats.range, out Obstaclehit, stats.range, obstacleMask))
        {
            if (Obstaclehit.collider != null)
            {
                canSee = false;
                Debug.DrawRay(fireLine.position, fld * Obstaclehit.distance, Color.red);
            }
            else
            {
                canSee = false;
                Debug.DrawRay(fireLine.position, fld * Obstaclehit.distance, Color.red);
            }

        }
        else if (Physics.Raycast(fireLine.position, fld * stats.range, out enemyhit, stats.range, targetMask))
        {
            if (compareTagsWith(enemyhit.collider))
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
        foreach(Product product in stats.products)
        {
            if(product.productionType == Production.Energy)
            {
                if (!isProducing)
                {
                    PlayerStats.EnGeneration += product.production;
                    isProducing = true;
                }
                    
            }
            else if(product.productionType == Production.Material){
                if (!isProducing)
                { 
                    PlayerStats.MatGeneration += product.production;
                    isProducing = true;
                }
            }else if(product.productionType == Production.Health)
            {
                targetStats = target.GetComponent<StructureStats>();
                targetStats.heal(product.production * Time.deltaTime);
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
