using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//a little list of production types
public enum Production
{
    Damage, // not implemented
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
    public Image healthBar;
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
        //turn off the structure if there is no energy.
        if(PlayerStats.Energy <= 0 && energyCost > 0)
        {
            isWorking = false;
            //if is core then stop production
            if (isCore)
            {
                StopCoreProduction();
            }

        }else if (!isWorking) // if there is energy and the structure is turned off then turn it on
        {
            isWorking = true;
            // if is core then start production (production of turrets are handled by the strategic turret controller script)
            if (isCore)
            {
                CoreProduce();
            }
        }

        //update healthbar if exist.
        if(healthBar != null)
        {
            healthBar.fillAmount = health / maxHealth;
        }

    }

    /// <summary>
    /// method called when the structure recive damage.
    /// </summary>
    /// <param name="amount"> Amount of damage that the structure will recive</param>
    public void TakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0 && !isDestroyed)
        {
            Explode();
        }
    }

    /// <summary>
    /// method called when the structure is healed / repaired
    /// </summary>
    /// <param name="amount"> amount of heal to recive</param>
    public void heal(float amount)
    {
        if((health + amount) > maxHealth)
        {
            heal(maxHealth-health);
            return;
        }
        health += amount;
    }

    /// <summary>
    /// Method called when the structure will explode / die
    /// </summary>
    void Explode()
    {
        isDestroyed = true;

        GameObject effect = Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
        Destroy(gameObject);
    }

    /// <summary>
    /// Method called to start the production of the core if the structure is a core
    /// </summary>
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

    /// <summary>
    /// Method called to stop the production of the core.
    /// </summary>
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

    /// <summary>
    /// Called when the structure is destroyed
    /// </summary>
    private void OnDestroy()
    {
        PlayerStats.EnConsumption -= energyCost;
        StopCoreProduction();
    }

    /// <summary>
    /// Called when you select the prefab on scene to show the structure range.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
