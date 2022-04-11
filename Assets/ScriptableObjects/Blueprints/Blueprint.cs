using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyGame/BluePrints/NodeBlueprint", fileName = "NodeBlueprint")]
public class Blueprint : ScriptableObject
{
    public string Name;
    public List<GameObject> prefabs;
    public Sprite Sprite;
}
