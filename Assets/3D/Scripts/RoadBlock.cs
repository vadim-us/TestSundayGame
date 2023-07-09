using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadBlock : MonoBehaviour
{
    [SerializeField]
    private List<lampPost> lights;

    [SerializeField]
    private GameObject obstacle;

    private void Awake()
    {
        lights.ForEach((light) =>
        {
            light.LightOn(Convert.ToBoolean(Random.Range(0, 2)));
        });

        GameObject obstacle = Instantiate(this.obstacle, transform);
        obstacle.transform.position = new Vector3(Random.Range(0, 10), 0, Random.Range(-10, 10));
        obstacle.transform.Rotate(new Vector3(0, 90, 0));
    }
}
