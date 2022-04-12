using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    private TurretBlueprint turretToBuild;
    private Nodo selectedNode;
    
    public static BuildManager instance;
    public GameObject BuildEffect;
    public GameObject SellEffect;
    public NodoUI nodoUI;
    public bool CanBuild { get { return turretToBuild != null; } }
    public bool HasMoney { get { return PlayerStats.materials >= turretToBuild.materialCost; } }
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one BuildManager in scene!");
            return;
        }
        instance = this;
    }
    public void SelectNode(Nodo nodo)
    {
        if (selectedNode == nodo)
        {
            DeselectNode();
            return; 
        }   
        selectedNode = nodo;
        turretToBuild = null;
        nodoUI.SetTarget(nodo);
    }

    public void DeselectNode()
    {
        selectedNode = null;
        nodoUI.Hide();
    }

    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        turretToBuild = turret;
        DeselectNode();
    }

    public TurretBlueprint GetTurretToBuild()
    {
        return turretToBuild;
    }

}
