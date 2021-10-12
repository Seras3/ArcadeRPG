using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float OX, OY, OZ;

    public float movingSpeed = 25f;

    void Start()
    {
        
    }

    void Update()
    {
        OX = Input.GetAxis("Horizontal");
        OZ = Input.GetAxis("Vertical");

        if (OX != 0 || OZ != 0) { transform.position += new Vector3(OX, OY, OZ) * Time.deltaTime * movingSpeed; }
    }

}
