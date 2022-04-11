using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorShop : MonoBehaviour
{
    //BuildManager buildManager;
    BuildingCreator buildingCreator;
    public Blueprint[] Blueprints;
    public Button ButtonPrefab;
    public Transform ShopContent;
    private void Start()
    {
        buildingCreator = BuildingCreator.GetInstance();
        //buildManager = BuildManager.instance;
        foreach (Blueprint blueprint in Blueprints)
        {
            Button button = Instantiate(ButtonPrefab, ShopContent);
            button.name = blueprint.Name;
            button.image.sprite = blueprint.Sprite;
            button.onClick.AddListener(delegate { SelectTurret(blueprint); });
        }
    }
    public void SelectTurret(Blueprint Blueprint)
    {
        buildingCreator.BlueprintSelected(Blueprint);
        //buildManager.SelectTurretToBuild(Blueprint);
    }
}
