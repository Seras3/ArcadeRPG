using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float forceMult = 500;
    new private Rigidbody rigidbody;

    
    
    private float floatingCount;
    private float floatingLower=3;
    private float floatingUpper=3.3f;
    private float floatingOffset=0.01F;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        
        floatingCount=floatingLower;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        var camera = Camera.main;
        var forward = camera.transform.forward;
        var right = camera.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        var desiredMoveDirection = forward * verticalAxis + right * horizontalAxis;


        transform.position =new Vector3 (transform.position.x, floatingCount, transform.position.z);
        floatingCount += floatingOffset;

        if (floatingCount >= floatingUpper || floatingCount <= floatingLower) floatingOffset=-floatingOffset;

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)){
            rigidbody.drag = 7;
        }
        else{
            rigidbody.drag = 0;
            rigidbody.velocity = desiredMoveDirection * Time.deltaTime * forceMult;
        }

        // if (Input.GetKey(KeyCode.W)){
        //     rigidbody.velocity = forward * Time.deltaTime * forceMult;
        // }
        // if (Input.GetKey(KeyCode.S)){
        //     rigidbody.velocity = -forward * Time.deltaTime * forceMult;
        // }
        // if (Input.GetKey(KeyCode.D))
        // {
        //     rigidbody.velocity = right * Time.deltaTime * forceMult;
        // }

        // if (Input.GetKey(KeyCode.A))
        // {
        //     rigidbody.velocity = -right * Time.deltaTime * forceMult;
        // } 
    }
}