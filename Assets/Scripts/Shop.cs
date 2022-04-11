using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    BuildManager buildManager;
    //BuildingCreator buildingCreator;
    public TurretBlueprint[] TurretBlueprints;
    public Button ButtonPrefab;
    public Transform ShopContent;
    private void Start()
    {
        //buildingCreator = BuildingCreator.GetInstance();
        buildManager = BuildManager.instance;
        foreach(TurretBlueprint turret in TurretBlueprints)
        {
            Button button = Instantiate(ButtonPrefab, ShopContent);
            button.name = turret.Name;
            button.image.sprite = turret.Sprite;
            button.onClick.AddListener(delegate { SelectTurret(turret); });
        }
    }
    public void SelectTurret(TurretBlueprint Blueprint)
    {
        //buildingCreator.BlueprintSelected(Blueprint);
        buildManager.SelectTurretToBuild(Blueprint);
    }
} 
