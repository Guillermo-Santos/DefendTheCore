using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int startMaterials = 150;
    public int maxEnergy = 100;
    [HideInInspector]
    public static float materials;
    public static int Rounds;
    public static float Energy;
    public static int MatGeneration = 0;
    public static int EnGeneration = 0;
    public static int EnConsumption = 0;
    // Start is called before the first frame update
    void Start()
    {
        materials = startMaterials;
        Energy = maxEnergy;
        Rounds = 0;
    }

    // Update is called once per frame
    void Update()
    {
        EnergyGeneration();
        MaterialGeneration();
    }

    public float getEnProduction()
    {
        return EnGeneration - EnConsumption;
    }

    public float getMatProduction()
    {
        return MatGeneration;
    }

    public float getEnergyAmount()
    {
        return Energy / maxEnergy;
    }
    void MaterialGeneration()
    {
        materials += (MatGeneration * Time.deltaTime);
    }

    void EnergyGeneration()
    {
        EnGenerate((getEnProduction() * Time.deltaTime));
    }

    void EnGenerate(float amount)
    {
        if ((Energy + amount) > maxEnergy)
        {
            Energy = maxEnergy;
            return;
        }
        if((Energy + amount) < 0)
        {
            if(Energy != 0)
                EnGenerate(-Energy);
            return;
        }
        
        Energy += amount;
    }
}
