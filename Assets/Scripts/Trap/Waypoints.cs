using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    Transform[] waypoints;

    int index = 0;

    public Transform CurrentWaypoint => waypoints[index];

    private void Awake()
    {
        waypoints = new Transform[transform.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = transform.GetChild(i);
        }
    }

    public Transform GetNextWaypoint()
    {
        index++;
        index %= waypoints.Length;

        return waypoints[index];
    }
}