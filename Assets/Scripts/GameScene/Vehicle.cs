using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class Vehicle : MonoBehaviour
{
    public enum State : int
    {
        None = 0,
        Right,
        Left,
        Up,
        Down
    }

    public State state = State.None;
    
    private float speed = 5.0f;
    private Vector3 maxPosZ;
    private Vector3 minPosZ;
    private Vector3 maxPosX;
    private Vector3 minPosX;
    private Vector3 dirZ;
    private Vector3 dirX;
    
    // Start is called before the first frame update
    void Start()
    {
        dirZ = Vector3.forward;
        dirX = Vector3.right;
        maxPosZ = new Vector3(0,0,75.0f);
        minPosZ = new Vector3(0,0,-75.0f);
        maxPosX = new Vector3(75.0f, 0, 0);
        minPosX = new Vector3(-75.0f, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.CompareTag("VehicleRight"))
        {
            state = State.Right;
        }
        if (gameObject.CompareTag("VehicleLeft"))
        {
            state = State.Left;
        }
        if (gameObject.CompareTag("VehicleUp"))
        {
            state = State.Up;
        }
        if (gameObject.CompareTag("VehicleDown"))
        {
            state = State.Down;
        }

        switch (state)
        {
            case State.None:
                break;
            case State.Right:
                UpdateRight();
                break;
            case State.Left:
                UpdateLeft();
                break;
            case State.Up:
                UpdateUp();
                break;
            case State.Down:
                UpdateDown();
                break;
        }
    }

    void UpdateRight()
    {
        transform.localPosition += dirZ * (speed * Time.deltaTime);
        if (transform.localPosition.z > maxPosZ.z)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -75.0f);
        }
    }

    void UpdateLeft()
    {
        transform.localPosition += -dirZ * (speed * Time.deltaTime);
        if (transform.localPosition.z < minPosZ.z)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 75.0f);
        }
    }

    void UpdateUp()
    {
        transform.localPosition += -dirX * (speed * Time.deltaTime);
        if (transform.localPosition.x < minPosX.x)
        {
            transform.localPosition = new Vector3(75.0f, transform.localPosition.y, transform.localPosition.z);
        }
    }

    void UpdateDown()
    {
        transform.localPosition += dirX * (speed * Time.deltaTime);
        if (transform.localPosition.x > maxPosX.x)
        {
            transform.localPosition = new Vector3(-75.0f, transform.localPosition.y, transform.localPosition.z);
        }
    }

}
