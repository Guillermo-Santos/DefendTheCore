using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Production
{
    Damage,
    Health,
    Energy,
    Material
}

public class StructureStats : MonoBehaviour
{
    [Header("Structure Stats")]
    public float maxHealth = 100f;
    public float range = 3f;
    public int energyCost = 5;

    [Header("Production")]
    public bool isHealer = false;
    public bool isCore = false;
    public List<Product> products = new List<Product>();

    [Header("Unity Objects")]
    public GameObject ExplosionEffect;
    [HideInInspector]
    public float health;
    public bool isWorking = false;
    bool isDestroyed = false;
    bool isCoreProducing = false;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        PlayerStats.EnConsumption += energyCost;
    }

    void Update()
    {
        if(PlayerStats.Energy <= 0 && energyCost > 0)
        {
            isWorking = false;
            if (isCore)
            {
                StopCoreProduction();
            }

        }else if (!isWorking)
        {
            isWorking = true;
            if (isCore)
            {
                CoreProduce();
            }
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0 && !isDestroyed)
        {
            Explode();
        }
    }

    public void heal(float amount)
    {
        if((health + amount) > maxHealth)
        {
            heal(maxHealth-health);
            return;
        }
        health += amount;
    }

    void Explode()
    {
        isDestroyed = true;

        GameObject effect = Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
        Destroy(gameObject);
    }
    void CoreProduce()
    {
        if (!isCoreProducing)
        {
            foreach (Product product in products)
            {
                if (product.productionType == Production.Energy)
                {
                    PlayerStats.EnGeneration += product.production;
                }
                else if (product.productionType == Production.Material)
                {
                    PlayerStats.MatGeneration += product.production;
                }
            }
            isCoreProducing = true;
        }
    }

    void StopCoreProduction()
    {
        if (isCoreProducing)
        {
            foreach (Product product in products)
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
            isCoreProducing = false;
        }
    }
    private void OnDestroy()
    {
        PlayerStats.EnConsumption -= energyCost;
        StopCoreProduction();
    }

}
