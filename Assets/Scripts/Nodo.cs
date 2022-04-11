using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;



public class Nodo : MonoBehaviour, IMouse
{
    public Color hoverColor;
    public Vector3 positionOffset;
    public Color BlockedColor;
    [HideInInspector]
    public GameObject turret;
    public TurretBlueprint turretBlueprint;
    public bool isUpgraded = false;

    private Renderer rend;
    private Color StartColor;

    BuildManager buildManager;

    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }

    void Start()
    {
        rend = GetComponent<Renderer>();
        StartColor = rend.material.color;
        buildManager = BuildManager.instance;
    }

    void BuildTurret(TurretBlueprint blueprint)
    {        
        if (!buildManager.HasMoney)
        {
            return;
        }
        PlayerStats.materials -= blueprint.cost;
        GameObject _turret = Instantiate(blueprint.prefabs[0], GetBuildPosition(), Quaternion.identity);
        turret = _turret;
        turretBlueprint = blueprint;
        GameObject effect = Instantiate(buildManager.BuildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 2f);
    }

    public void UpgradeTurret()
    {
        if (PlayerStats.materials < turretBlueprint.upgradedCost)
        {
            return;
        }
        PlayerStats.materials -= turretBlueprint.upgradedCost;
        //Destroy old turret
        Destroy(turret);
        //Build Upgraded turret
        GameObject _turret = Instantiate(turretBlueprint.prefabs[1], GetBuildPosition(), Quaternion.identity);
        turret = _turret;
        turretBlueprint.cost += turretBlueprint.upgradedCost;

        GameObject effect = Instantiate(buildManager.BuildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 2f);
        isUpgraded = true;
    }
    public void SellTurret()
    {
        PlayerStats.materials += turretBlueprint.GetSellAmount();

        GameObject effect = Instantiate(buildManager.SellEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 2f);

        //Destroy old turret
        Destroy(turret);
        turretBlueprint = null;



        isUpgraded = false;
    }

    void IMouse.OnMouseEnter()
    {

        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (!buildManager.CanBuild)
            return;
        if (buildManager.HasMoney)
        {
            rend.material.color = hoverColor;
        }else
        {
            rend.material.color = BlockedColor;
        }
    }

    void IMouse.OnMouseExit()
    {
        rend.material.color = StartColor;
    }

    void IMouse.OnMouseDown()
    {

        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (turret != null)
        {
            buildManager.SelectNode(this);
            return;
        }
        if (!buildManager.CanBuild)
            return;

        BuildTurret(buildManager.GetTurretToBuild());
    }


}
