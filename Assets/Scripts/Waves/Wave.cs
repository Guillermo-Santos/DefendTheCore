using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public List<WaveEnemy> enemies;
    public int count;
    public float spawnRate;
}
