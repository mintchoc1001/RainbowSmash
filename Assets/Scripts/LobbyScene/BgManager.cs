using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Firebase.Database;
using UnityEngine;

public class BgManager : MonoBehaviour
{
    public Material skybox;
    private static readonly int Rotation = Shader.PropertyToID("_Rotation");

    private float offset;
    private float speed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        offset = 0;
    }

    // Update is called once per frame
    void Update()
    {
        offset += Time.deltaTime * speed;
        if (offset >= 360.0f)
        {
            offset = 0;
        }
        
        skybox.SetFloat(Rotation, offset);
    }
}
