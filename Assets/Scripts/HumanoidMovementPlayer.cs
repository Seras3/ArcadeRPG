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

    void MoveCharacter(){
        
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

    void TurnTowardsMouse(){
        
        //Vector3 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        //mousePosition.z = 0f;

        Plane plane = new Plane(Vector3.up, transform.position);        

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float dist;
        if (plane.Raycast(ray, out dist))
        {
            //Debug.Log(ray.GetPoint(dist));
            
            //find the vector pointing from our position to the target
            Vector3 direction = (ray.GetPoint(dist) - transform.position).normalized;
    
            //create the rotation we need to be in to look at the target
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
    // Update is called once per frame
    void Update()
    {
        MoveCharacter();
        TurnTowardsMouse();
    }

    void FixedUpdate() {   
    }
}
