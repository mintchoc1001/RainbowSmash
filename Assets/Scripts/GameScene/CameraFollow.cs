using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    //public float height = 5.0f;
    //public float distance = 10.0f;
    //public float angle = 45.0f;
    //public float lookAtHeight = 2.0f;
    //public float smoothSpeed = 0.5f;

    //private Vector3 refVelocity;

    private void Start()
    {
        HandleCamera();
    }
    void Update()
    {
        HandleCamera();
    }

    private void HandleCamera()
    {
        if (!target)
        {
            return;
        }
        transform.position = target.transform.position + offset;
        //Vector3 worldPosition = (Vector3.forward * -distance) + (Vector3.up * height);
        //Vector3 rotatedVector = Quaternion.AngleAxis(angle, Vector3.up) * worldPosition;
        //Vector3 finalTargetPosition = target.position;
        //finalTargetPosition.y += lookAtHeight;
        //Vector3 finalPosition = finalTargetPosition + rotatedVector;

        //transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, smoothSpeed);

        //transform.LookAt(target.position);
    }
}
