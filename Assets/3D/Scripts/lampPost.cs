using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lampPost : MonoBehaviour
{
    [SerializeField]
    private new MeshRenderer light;

    [SerializeField]
    private bool lightIsOn;

    public void LightOn(bool on)
    {
        lightIsOn = on;

        light.material.SetInt("_OnLights", lightIsOn ? 1 : 0);
        light.GetComponentInChildren<Light>().enabled = lightIsOn;
    }

    private void Awake()
    {
        LightOn(lightIsOn);
    }
}
