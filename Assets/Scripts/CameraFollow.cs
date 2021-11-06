using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Camera orthoCam;
    public Transform target;
    public Vector3 offset;
    public float zoomSpeed;
    void Start()
    {
        orthoCam = Camera.main;
        orthoCam.enabled = true;
        orthoCam.orthographicSize = 9;
        target = GameObject.Find("Dummy").transform;
        offset.x = 16.5f;
        offset.z = -5.7f;
        offset.y = 15;
        zoomSpeed = 10f;
    }

    void MoveCamera(){
        transform.position = new Vector3 (target.position.x, 0, target.position.z)+ offset;
        transform.LookAt(target);
    }

    void HandleZoom(){
        if (Input.GetKey(KeyCode.Y)){
            orthoCam.orthographicSize -= zoomSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.U)){
            orthoCam.orthographicSize += zoomSpeed * Time.deltaTime;
        }
    }
    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        HandleZoom();
    }
}
