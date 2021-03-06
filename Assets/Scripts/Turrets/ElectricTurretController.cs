using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTurretController : MonoBehaviour
{
    public StructureStats stats;

    public string Objetive_Tag = "Enemy";
    [Range(0f, 1f)]
    public float SlowPct = .75f;
    [HideInInspector]
    public List<EnemyStats> targets;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        stats = GetComponent<StructureStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stats.isWorking)
        {
            UpdateTarget();
            SlowTargets();
        }
    }

    void UpdateTarget()
    {
        targets.Clear();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(Objetive_Tag);
        float distanceToEnemy;
        foreach (GameObject enemy in enemies)
        {
            distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= stats.range)
            {
                targets.Add(enemy.GetComponent<EnemyStats>());
            }
        }
    }

    void SlowTargets()
    {
        if (targets.Count > 0)
        {
            if(!audioSource.isPlaying)
                audioSource.Play();
            foreach (EnemyStats target in targets)
            {
                target.Slow(SlowPct);
            }
        }
    }
}
