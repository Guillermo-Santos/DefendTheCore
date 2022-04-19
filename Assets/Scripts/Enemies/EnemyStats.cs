using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float startHealth = 100f;
    public int MoneyDrop = 50;
    public float startSpeed = 2f;
    public int impactDamage = 5;

    [Header("Enemy Movement")]
    [Range(0f, 2f)]
    public float floorDistance = 1f;
    public LayerMask moveOverMask;
    public Targets Target = Targets.WayPoints;
    public bool canMove = true;
    public bool canAtack = false;
    [Header("Enemy Atack")]
    public Transform firePoint;
    public LayerMask targetMask;
    public string Objetive_Tag;
    public Transform lastObjetive;
    public GameObject bullet;
    [Range(0f,10f)]
    public float range;
    public float fireRate = 1f;
    public float fireCountdown = 1f;

    [Header("Unity Objects")]
    public GameObject DeadEffect;
    public Image healthBar;

    [HideInInspector]
    public float health;
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public string CoreTag;
    private bool isDead = false;
    private void Start()
    {
        speed = startSpeed;
        health = startHealth;
        if(lastObjetive != null)
            CoreTag = lastObjetive.tag;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        healthBar.fillAmount = health / startHealth;
        if (health <= 0 && !isDead)
        {
            Die();
        }
    } 

    public void Slow(float pct)
    {
        speed = startSpeed * (1f - pct);
    }

    void Die()
    {
        isDead = true;

        GameObject effect = Instantiate(DeadEffect,transform.position,Quaternion.identity);
        Destroy(effect,1.5f);
        PlayerStats.materials += MoneyDrop;
        WaveSpawner.EnemiesAlive--;
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
