using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorManager : MonoBehaviour
{
    public Transform ParentContainer;
    public GameObject ObjectToBuild;
    public Text X;
    public Text Z;

    public Task SearchAndDestroy()
    {
        int a = ParentContainer.childCount;
        for (int i = 0; i < a; i++)
        {
            Destroy(ParentContainer.GetChild(i).GetComponent<Transform>().gameObject);
        }
        ParentContainer.position = Vector3.zero;
        return Task.CompletedTask;
    }
   
    public async void Generate()
    {
        await SearchAndDestroy();
        int x = Convert.ToInt32(X.text);
        int z = Convert.ToInt32(Z.text);
        int cont = 0;
        GameObject gObject;
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                cont++;
                gObject = Instantiate(ObjectToBuild, ParentContainer.position, Quaternion.identity);
                gObject.name = ObjectToBuild.name + $"({cont})";
                gObject.transform.parent = ParentContainer.transform;
                gObject.transform.position = new Vector3(i, ParentContainer.position.y, j);
            }
        }
        ParentContainer.position = new Vector3(-x / 2, ParentContainer.position.y, -z / 2);
    }
}
