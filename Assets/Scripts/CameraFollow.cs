using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;
    public float fixedY = 10;
    void Start()
    {
        target = GameObject.Find("Dummy").transform;
        offset.x = 18.3f;
        offset.z = -6.3f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3 (target.position.x, fixedY, target.position.z)+ offset;
    }
}
