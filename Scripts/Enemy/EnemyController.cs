using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public CharacterController enemyController;
    public CharacterController Player;
    public Animator animator;
    PlayerController playerController;

    private float targetAngle;
    private float angleToPlayer;
    public float moveSpeed;
    float turnSmoothVelocity;
    public float turnTime;
    public float enemyDetectionDistance;
    private float distance;
    private Vector3 desiredMovement;

    float groundedGravity = -0.05f;
    public float gravity = -9.8f;


    public float attackDistance;
    public float hitRange;
    public float attackDamage;
    public bool attacking = false;
    public float attackAngle = 30;
    public bool alive = true;

    bool blocked;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerController = Player.GetComponent<PlayerController>();

    }


    void Update()
    {
        if (!alive)
        {
            animator.enabled = false;
            this.enabled = false;
        }

        distance = (Player.transform.position - transform.position).magnitude;
        angleToPlayer = Mathf.Atan2(playerController.transform.position.x - transform.position.x, playerController.transform.position.z - transform.position.z) * Mathf.Rad2Deg;
        if (angleToPlayer < 0)
        {
            angleToPlayer += 360; 
        }
        if (distance <= attackDistance &&  Mathf.Abs(angleToPlayer-transform.eulerAngles.y) <= attackAngle)
        {
            
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", true);
        }
        else if (distance < enemyDetectionDistance && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);


            targetAngle = Mathf.Atan2(Player.transform.position.x - transform.position.x, Player.transform.position.z - transform.position.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            desiredMovement = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward * moveSpeed;
            if (enemyController.isGrounded)
            {
                desiredMovement.y = groundedGravity;
            }
            else
            {
                desiredMovement.y += gravity * Time.deltaTime;
            }

            enemyController.Move(desiredMovement * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isWalking", false);
        }
    }

    public void hitPlayer()
    {
        if (distance <= hitRange && Mathf.Abs(angleToPlayer - transform.eulerAngles.y) <= attackAngle)
        {
            playerController.damaged(attackDamage, transform.position);
        }
    }



}
