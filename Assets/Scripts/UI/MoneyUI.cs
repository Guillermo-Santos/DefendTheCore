using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    public Text Money;
    // Update is called once per frame
    void Update()
    {
        Money.text = "$" + PlayerStats.materials;
    }
}
