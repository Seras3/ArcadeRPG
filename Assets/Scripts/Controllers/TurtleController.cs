using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Stats;

public class TurtleController : EnemyController
{
    private float delayTimeState = 7;
    private float lastStateChange;
    private int state;
    private int lastState;
    private Vector3 playerPosition;
    private Plane _plane;
    private Vector3 lockedPlayerPosition;
    private Color walkColor = Color.white;
    private Color chargeColor = Color.red;  
    private Color lerpedColor = Color.white;
    
    private int _baseDamage;
    
    private float colorValue;

    void Start()
    { 
        Anim = GetComponent<Animator>();

        _plane = new Plane(Vector3.up, 0);

        interpolant = 0.1f;

        enemyStats = GetComponent<Stats.TurtleStats>(); 

        lastStateChange = Time.time;

        _baseDamage = enemyStats.Damage;
        
        state = 1;

        colorValue = 0;
    }

    void FixedUpdate()
    {
        if (GameManager.CurrentStatus != GameManager.GameStatus.Playing) return;

        playerPosition = GameObject.Find("B-spine").transform.position;

        if (playerPosition != null) 
        {
            var lookPos = playerPosition - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);
        }

        if (Time.time - lastStateChange > delayTimeState)
        {
            ChangeState();
        }

        if (state == 1) 
        {
            if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("GetHit")) 
            {
                Anim.Play("WalkFWD", 0);
                base.MoveTowardsPlayer();
            }            
        }

        if (state == 2) 
        {
            
            Anim.Play("Taunt", 0);
            base.MoveTowardsPlayer();

            if (lastState == 1) {
                colorValue += 0.001f;
            }
            else {
                colorValue -= 0.005f;
            }

            lerpedColor = Color.Lerp(walkColor, chargeColor,  colorValue);

            foreach(var rend in GetComponentsInChildren<Renderer>(true))
            {
                rend.material.color = lerpedColor;
            }

            if (colorValue > 0.999f) {
                colorValue = 1;
                ChangeState();
            }
            else if (colorValue < 0.001f) {
                colorValue = 0;
                ChangeState();
            }

        }

        if (state == 3)
        {
            FindObjectOfType<AudioManager>().Play("turtleAttack");
            Anim.Play("Attack01", 0);
            Charge();
        }
    }

    void ChangeState()
    {
        lastStateChange = Time.time;

        if (state == 1) {
            state = 2;
            lastState = 1;
        }
        else if (state == 3) {
            state = 2;
            lastState = 3;
        }
        else if (state == 2 && lastState == 1) {
            state = 3;
            lockedPlayerPosition = GameObject.Find("B-spine").transform.position;
        }
        else {
            state = 1;
        }
    }


    void Charge()
    {
        Vector3 enemyPosition = transform.position;

        
        lockedPlayerPosition.y = enemyPosition.y;
        transform.position = Vector3.MoveTowards(enemyPosition, lockedPlayerPosition, ((TurtleStats)enemyStats).MovementSpeedOnCharge);
        enemyStats.Damage = ((TurtleStats) enemyStats).DamageOnCharge;
        if (transform.position == lockedPlayerPosition)
        {
            enemyStats.Damage = _baseDamage;
            ChangeState();
        }
    }

}