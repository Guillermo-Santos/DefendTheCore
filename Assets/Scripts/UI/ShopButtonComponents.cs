using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopButtonComponents : MonoBehaviour
{

    public TextMeshProUGUI materialCost;
    public TextMeshProUGUI energyCost;

    public void setComponents(string materialCost, string energyCost)
    {
        this.materialCost.text = materialCost;
        this.energyCost.text = energyCost;
    }
    
}
