using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField]
    private RoadBlock road;

    private Car car;


    void Start()
    {
        for (int i = 1; i <= 30; i++)
        {
            GameObject road = Instantiate(this.road.gameObject, transform);
            road.transform.position = new Vector3(i * 20, 0, 0);
        }
    }


    void Update()
    {

    }
}
