using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Targets
{
    WayPoints,
    StrategicTurrets
}

[RequireComponent(typeof(EnemyStats))] 
public class EnemyMovement : MonoBehaviour
{   
    private Transform target;

    private int wavepointIndex;

    private EnemyStats stats;
    private Vector3 Direction;
    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<EnemyStats>();
        if(stats.Target == Targets.WayPoints)
        {
            resetTarget();
        }
        else if(stats.Target == Targets.StrategicTurrets)
        {
            InvokeRepeating("SearchForTurretTargets", 0f, 0.5f);
        }
    }

    void resetTarget()
    {
        wavepointIndex = 0;
        target = WayPoints.points[wavepointIndex];
        setDirection();
    }

    void SearchForTurretTargets()
    {
        GameObject[] turrets = GameObject.FindGameObjectsWithTag(stats.Objetive_Tag);
        if(turrets.Length <= 0)
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

        if (nearestTurret != null)
        {
            target = nearestTurret.transform;
        }
        else
        {
            stats.canMove = false;
            //target = stats.lastObjetive.transform;
        }

        if(stats.canMove)
            setDirection();

    }

    void Update()
    {
        HeightControl();

        if (stats.canMove)
        {
            Utility.LookOnTarget(transform,Direction,stats.speed);
            Move();

            if(stats.Target == Targets.WayPoints)
            {
                if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(target.position.x, target.position.z)) <= 0.3f)
                {
                    getNextWaypoint();
                }
            }
        }
        else
        {
            Stop();
        }
        

        stats.speed = stats.startSpeed;
    }

    void HeightControl()
    {
        DownControl();
        FwdControl();
        
    }

    void DownControl()
    {
        Vector3 down = transform.TransformDirection(Vector3.down);
        RaycastHit Downhit;
        if (Physics.Raycast(transform.position, down * 2, out Downhit, 2, stats.moveOverMask))
        {
            if (Downhit.collider != null)
            {
                float Distance = Vector3.Distance(Downhit.collider.transform.position, transform.position);
                if (Distance < stats.floorDistance)
                {
                    Direction.y = Direction.y + stats.floorDistance;
                    MoveUp(new Vector3(transform.position.x, Direction.y + stats.floorDistance, transform.position.z));
                    Debug.DrawRay(transform.position, down * Downhit.distance, Color.yellow);
                }
                else
                {
                    Stop();
                    setDirection();
                    Debug.DrawRay(transform.position, down * Downhit.distance, Color.green);
                }
            }
            else
            {
                Debug.DrawRay(transform.position, down * stats.floorDistance, Color.red);
            }

        }
        else
        {
            Debug.DrawRay(transform.position, down * stats.floorDistance, Color.red);
        }
    }

    void FwdControl()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit Fwdhit;

        if (Physics.Raycast(transform.position, fwd * 2, out Fwdhit, 2, stats.moveOverMask))
        {
            if (Fwdhit.collider != null)
            {
                Direction.y = Direction.y + stats.floorDistance;
                MoveUp(new Vector3(transform.position.x,Direction.y+stats.floorDistance, transform.position.z));
                Debug.DrawRay(transform.position, fwd * Fwdhit.distance, Color.yellow);
            }
            else
            {
                Stop();
                setDirection();
                Debug.DrawRay(transform.position, fwd * Fwdhit.distance, Color.green);
            }

        }
        else
        {
            Debug.DrawRay(transform.position, fwd * stats.floorDistance, Color.red);
        }
    }

    void Move()
    {
        transform.Translate(Direction.normalized * stats.speed * Time.deltaTime, Space.World);
    }

    void MoveUp(Vector3 pos)
    {
        transform.Translate(pos.normalized * stats.speed * Time.deltaTime, Space.World);
    }

    private void Stop()
    {
        transform.Translate(Direction.normalized * 0, Space.World);
    }

    void setDirection()
    {
        Direction = target.position - transform.position;
    }

    void getNextWaypoint()
    {
        if (wavepointIndex >= WayPoints.points.Length - 1)
        {
            EndPath();
            return;
        }

        wavepointIndex++;
        target = WayPoints.points[wavepointIndex];
        setDirection();
    }

    void EndPath()
    {
        //PlayerStats.Lives--;
        //WaveSpawner.EnemiesAlive--;
        //Destroy(gameObject);
        // This line is for testing. it make the enemy to restart from the firstwaypoint
        resetTarget();
    }

}
