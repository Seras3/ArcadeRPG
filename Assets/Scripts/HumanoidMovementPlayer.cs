using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidMovementPlayer : MonoBehaviour
{

    public float movementSpeed;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        movementSpeed = 1;
        animator = GetComponent<Animator>();
        animator.speed = 0.65f*movementSpeed;
    }


    void setMovement(float horizontal, float vertical){
        
        if (horizontal==0 && vertical==0){
            animator.SetBool("isWalking", false);
        }
        else{
            animator.SetBool("isWalking", true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        setMovement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

    }
}
