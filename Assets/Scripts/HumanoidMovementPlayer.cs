using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidMovementPlayer : MonoBehaviour
{

    public float movementSpeed;

    private Animator animator;

    new private Camera camera;
    
    private Vector3 forward;
    private Vector3 right;
    // Start is called before the first frame update
    void Start()
    {
        movementSpeed = 6;
        animator = GetComponent<Animator>();
        animator.speed = 0.65f*movementSpeed/6;

        camera = Camera.main;
        forward = camera.transform.forward;
        right = camera.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
    }


    void setMovement(float horizontal, float vertical){
        
        if (horizontal==0 && vertical==0){
            animator.SetBool("isWalking", false);
        }
        else{
            animator.SetBool("isWalking", true);

            Vector3 desiredMoveDirection = forward * vertical + right * horizontal;
            transform.position = transform.position + desiredMoveDirection*Time.deltaTime*movementSpeed;
        }
    }
    // Update is called once per frame
    void Update()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        
     
        setMovement(horizontalAxis, verticalAxis);


    }
}
