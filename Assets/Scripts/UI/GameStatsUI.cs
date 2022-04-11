using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameStatsUI : MonoBehaviour
{
    public PlayerStats stats;

    public TextMeshProUGUI EnergyAmount;
    public TextMeshProUGUI EnergyGeneration;
    public TextMeshProUGUI MineralAmount;
    public TextMeshProUGUI MineralGeneration;

    public Image EnergyBar;

    // Update is called once per frame
    void Update()
    {
        EnergyBar.fillAmount = stats.getEnergyAmount();
        EnergyAmount.text = string.Format("{0:N0}", PlayerStats.Energy);
        EnergyGeneration.text = textFormater(stats.getEnProduction());
        MineralAmount.text = string.Format("{0:N0}", PlayerStats.materials);
        MineralGeneration.text = textFormater(stats.getMatProduction());
    }

    string textFormater(float amount)
    {
        if(amount == 0)
        {
            return "<color=#" + ColorUtility.ToHtmlStringRGB(Color.white) + "> - </color>";
        }
        else if(amount > 0)
        {
            return "<color=#" + ColorUtility.ToHtmlStringRGB(Color.green) + "> +" + amount + "</color>";
        }
        else
        {
            return "<color=#" + ColorUtility.ToHtmlStringRGB(Color.red) + "> " + amount + "</color>";
        }
    }
}
