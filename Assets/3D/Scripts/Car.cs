using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField]
    private List<MeshRenderer> lights;

    private bool lightIsOn = false;

    /// <summary>
    /// включение / выключение фар 
    /// </summary>
    public void LightOn()
    {
        lights.ForEach((light) =>
        {
            light.material.SetInt("_OnLights", lightIsOn ? 1 : 0);
            light.GetComponentInChildren<Light>().enabled = lightIsOn;
        });

        lightIsOn = !lightIsOn;
    }

    private void Awake()
    {
        LightOn();
    }



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
