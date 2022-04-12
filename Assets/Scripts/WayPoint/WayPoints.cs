using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public static List<Transform> points;
    private List<Transform> pointsList = new List<Transform>();
    void Awake()
    {
        InvokeRepeating("updatePoints",0f,2f);
    }

    void updatePoints()
    {
        pointsList.Clear();

        int points = transform.childCount;
        for (int i = 0; i < points; i++)
        {
            this.pointsList.Add(transform.GetChild(i));
        }

        WayPoints.points = pointsList;
    }

}
