using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "MyGame/BluePrints/TurretBlueprint", fileName ="TurretBlueprint")]
public class TurretBlueprint : Blueprint
{
    public int materialCost;
    public int upgradedCost;

    public int GetSellAmount()
    {
        return materialCost / 2;
    }

    public int MaxLevel => prefabs.Count;

}
