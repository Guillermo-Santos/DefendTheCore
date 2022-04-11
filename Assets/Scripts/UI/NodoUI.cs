
using UnityEngine;
using UnityEngine.UI;

public class NodoUI : MonoBehaviour
{
    public GameObject ui;
    [Header("Turret Info")]
    public Text turretName;
    public Text turretRange;
    public Text turretDamage;
    [Header("Upgrade Info")]
    public Button upgradeButton;
    public Text upgradeCost;
    [Header("Sell Info")]
    public Button sellButton;
    public Text sellGains;

    private Nodo target;
    public void SetTarget(Nodo nodo)
    {
        target = nodo;
        transform.position = target.GetBuildPosition();
        if (!target.isUpgraded)
        {
            upgradeCost.text = "$" + target.turretBlueprint.upgradedCost;
            upgradeButton.interactable = true;
        }
        else
        {
            upgradeCost.text = "DONE";
            upgradeButton.interactable = false;
        }
        sellGains.text = "$" + target.turretBlueprint.GetSellAmount();
        SetTurretInfo(nodo.turret.GetComponent<TurretController>());
        ui.SetActive(true);
    }

    public void SetTurretInfo(TurretController turret)
    {
        turretName.text = target.turretBlueprint.Name;
        turretRange.text = "Range: " + turret.stats.range;
        if (turret.useLaser)
        {
            turretDamage.text = "Damage: " + turret.damageOverTime + "/s";
        }
        else
        {
            turretRange.text = turretRange.text + "\nFire rate: " + turret.fireRate + "/s";
            turretDamage.text = "Damage: " + turret.bullet.GetComponent<Bullet>().damage;
        }
    }

    public void Hide()
    {
        ui.SetActive(false);
    }

    public void Upgrade()
    {
        target.UpgradeTurret();
        BuildManager.instance.DeselectNode();
    }

    public void Sell()
    {
        target.SellTurret();
        BuildManager.instance.DeselectNode();
    }

}
