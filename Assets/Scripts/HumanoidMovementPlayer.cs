using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HumanoidMovementPlayer : MonoBehaviour
{

    private CharacterController controller;

    public float movementSpeed;

    private Animator animator;

    new private Camera camera;
    
    public Vector3 moveDirection, lookDirection;
    private Vector3 forward;
    private Vector3 right;

    private float playerChestLevelY;
    private Plane chestPlane;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        movementSpeed = 6;
        animator = GetComponent<Animator>();
        animator.speed = 0.65f*movementSpeed/6;

        camera = Camera.main;
        forward = camera.transform.forward;
        right = camera.transform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();


        playerChestLevelY = gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).position.y;
        chestPlane = new Plane(Vector3.up, new Vector3(transform.position.x, playerChestLevelY, transform.position.z));
    }

    void MoveCharacter(){
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal==0 && vertical==0){
            animator.SetBool("isWalking", false);
        }
        else{
            animator.SetBool("isWalking", true);

            moveDirection = (forward * vertical + right * horizontal).normalized;
            controller.Move(moveDirection*Time.deltaTime*movementSpeed);
        }
    }

    void TurnTowardsMouse(){        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float dist;
        if (chestPlane.Raycast(ray, out dist))
        {                        
            lookDirection = (ray.GetPoint(dist) - new Vector3(transform.position.x, playerChestLevelY, transform.position.z)).normalized;
            
            transform.rotation = Quaternion.LookRotation(lookDirection);
            
            float angleBetweenLookAndMove = Vector3.SignedAngle(moveDirection, lookDirection, Vector3.up);

            animator.SetFloat("yRotation", angleBetweenLookAndMove);
        }
    }

    void Update()
    {
        MoveCharacter();
        TurnTowardsMouse();
    }

    void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            Vector3 pushBack = (collider.gameObject.transform.position - transform.position).normalized;
            pushBack.y = 0;
            collider.gameObject.transform.position += pushBack;
        }
    }
}
