using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiclePool : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("VehicleRight") || other.gameObject.CompareTag("VehicleLeft") ||
            other.gameObject.CompareTag("VehicleUp") || other.gameObject.CompareTag("VehicleDown"))
        {
            other.GetComponent<MeshRenderer>().enabled = false;
            MeshRenderer[] wheels = other.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("VehicleRight") || other.gameObject.CompareTag("VehicleLeft") ||
            other.gameObject.CompareTag("VehicleUp") || other.gameObject.CompareTag("VehicleDown"))
        {
            other.GetComponent<MeshRenderer>().enabled = true;
            MeshRenderer[] wheels = other.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].enabled = true;
            }
        }
    }
}
