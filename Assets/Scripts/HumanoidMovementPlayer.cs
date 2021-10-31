using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HumanoidMovementPlayer : MonoBehaviour
{

    private CharacterController controller;

    public float movementSpeed;

    private Animator animator;

    new private Camera camera;
    
    private Vector3 forward;
    private Vector3 right;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

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


    void moveCharacter(){
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal==0 && vertical==0){
            animator.SetBool("isWalking", false);
        }
        else{
            animator.SetBool("isWalking", true);

            float inverseSQRT = 1/Mathf.Sqrt(horizontal*horizontal+vertical*vertical);
            horizontal *= inverseSQRT;
            vertical *= inverseSQRT;

            Vector3 desiredMoveDirection = forward * vertical + right * horizontal;
            controller.Move(desiredMoveDirection*Time.deltaTime*movementSpeed);
        }
    }
    // Update is called once per frame
    void Update()
    {
        moveCharacter();

    }

    void FixedUpdate() {   
    }
}
