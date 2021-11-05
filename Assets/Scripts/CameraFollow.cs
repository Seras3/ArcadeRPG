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
        zoomSpeed = 300f;
    }

    void MoveCamera(){
        transform.position = new Vector3 (target.position.x, 0, target.position.z)+ offset;
        transform.LookAt(target);
    }

    void CheckZoom(){ //WIP
        float elapsed = Time.deltaTime;
        //float currentOrthoSize = orthoCam.orthographicSize;
        orthoCam.orthographicSize = Mathf.Lerp(orthoCam.orthographicSize, orthoCam.orthographicSize-(1*Input.mouseScrollDelta.y), elapsed);
    }
    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        CheckZoom();
    }
}
