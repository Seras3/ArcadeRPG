using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float OX, OY, OZ;

    public float movingSpeed = 25f;

    Vector2 mousePos;
    Vector2 positionOnScreen;

    void Start()
    {
        
    }

    void Update()
    {
        OX = Input.GetAxis("Horizontal");
        OZ = Input.GetAxis("Vertical");

        if (OX != 0 || OZ != 0) { transform.position += new Vector3(OX, OY, OZ) * Time.deltaTime * movingSpeed; }

        // Get positions of mouse and camera to calculate angle of the bullet
        mousePos = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
    }

    void FixedUpdate()
    {
        // Rotate player in the direction of the mouse
        Vector2 lookDir = mousePos - positionOnScreen;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(new Vector3(0f, -angle, 0f));
    }

}
