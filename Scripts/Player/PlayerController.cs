using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    CharacterController characterController;
    PlayerInput playerInput;
    Animator animator;

    public Transform cam;
  
    public float runSpeed;
    public float walkSpeed;
    public float jumpVel;

    public float gravity;
    float groundedGravity = -0.05f;

    float turnSmoothVelocity;
    float targetAngle;
    public float turnTime;

    float yVel;

    bool movementInputGiven;
    bool running;

    bool grounded;
    bool jumpPressed;
    bool jumping;
    public float jumpCD;
    float jumpCDCounter = 0;
    bool canJump = false;


    bool leftClick;
    bool rightClick;
    bool isFixed;

    bool dodgePressed;
    bool dodging;
    public float dodgeCD;
    float dodgeCDCounter = 0;
    bool canDodge = false;

    Vector2 movementInput;
    Vector3 desiredMovement;


    public bool blocking;
    public float blockAngle;
    float angleToEnemy;

    public float health;
    public float maxHealth;


    public TextMeshProUGUI HealthDisplay;

    


    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();


        // set the player input callbacks
        playerInput.PlayerControls.Movement.started += AssignMovementInput;
        playerInput.PlayerControls.Movement.canceled += AssignMovementInput;
        playerInput.PlayerControls.Movement.performed += AssignMovementInput;


        playerInput.PlayerControls.Run.started += inp => { running = inp.ReadValueAsButton(); };
        playerInput.PlayerControls.Run.canceled += inp => { running = inp.ReadValueAsButton(); };

        playerInput.PlayerControls.Dodge.started += inp => { Dodge(); };

        playerInput.PlayerControls.Jump.started += inp => { Jump(); };

        playerInput.PlayerControls.Attack.started += inp => { SwingSword(); Debug.Log("SWING"); };//leftClick = inp.ReadValueAsButton(); };
        //playerInput.PlayerControls.Attack.canceled += inp => { leftClick = inp.ReadValueAsButton(); };

        playerInput.PlayerControls.Block.started += inp => { RaiseShield(); };//rightClick = inp.ReadValueAsButton(); };
        playerInput.PlayerControls.Block.canceled += inp => { LowerShield(); };//rightClick = inp.ReadValueAsButton(); };


    }

    // handler function to set the player input values
    void AssignMovementInput(InputAction.CallbackContext inp)
    {
        movementInput = inp.ReadValue<Vector2>();
        movementInputGiven = movementInput.x != 0f || movementInput.y != 0f;
    }


    private void Start()
    {
        health = maxHealth;
    }


    private void RaiseShield()
    {
        if (!dodging && !isFixed && grounded)
        {
            isFixed = true;
            animator.SetBool("isBlocking", true);
        }
    }

    private void LowerShield()
    {
        animator.SetBool("isBlocking", false);
    }

    public void ShieldRaised()
    {
        blocking = true;
    }
    public void ShieldLowered()
    {
        blocking = false;
        isFixed = false;
    }

    private void SwingSword()
    {
        Debug.Log(dodging + ", " + isFixed + ", "+ grounded);
        if (!dodging && !isFixed && grounded)
        {
            animator.SetTrigger("Attack");
            isFixed = true;
        }
    }

    public void EndOfSwordSwing()
    {
        isFixed = false;
    }




    public void damaged(float damage, Vector3 location)
    {
        angleToEnemy = Mathf.Atan2(location.x - transform.position.x, location.z - transform.position.z) * Mathf.Rad2Deg;
        if (angleToEnemy < 0)
        {
            angleToEnemy += 360;
        }
        if (!blocking || !(Mathf.Abs(transform.eulerAngles.y - (Mathf.Atan2(location.x - transform.position.x, location.z - transform.position.z) * Mathf.Rad2Deg)) <= blockAngle))
        {
            health -= damage;
        }
    }


    private void Dodge()
    {
        if (canDodge)
        {
            animator.SetTrigger("Dodge");
            dodging = true;
            canDodge = false;
        }       
    }

    public void EndOfDodge()
    {
        dodging = false;

    }


    private void Jump()
    {
        if (!isFixed && grounded && canJump && !dodging)
        {
            jumping = true;
            canJump = false;
            desiredMovement.y = jumpVel;
            animator.SetBool("isJumping", true);
            animator.SetTrigger("Jump");
        }
    }


    




    void Update()
    {
        if (health <= 0)
        {
            Debug.Log("YOU DIED");
        }
        //DISPLAY HEALTH
        HealthDisplay.text = health.ToString();

        desiredMovement.x = 0;
        desiredMovement.z = 0;
        yVel = desiredMovement.y;

        //MOVEMENT
        
        if (!dodging && !isFixed)
        {
            if (movementInputGiven)
            {
                targetAngle = Mathf.Atan2(movementInput.x, movementInput.y) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;

                float heading = transform.eulerAngles.y;

                if (targetAngle < 0)
                {
                    targetAngle += 360;
                }
                if (heading < 0)
                {
                    heading += 360;
                }


                animator.SetBool("isWalking", true);
                if (running)
                {
                    desiredMovement = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward * runSpeed;
                    animator.SetBool("isRunning", true);
                }
                else
                {
                    desiredMovement = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward * walkSpeed;
                    animator.SetBool("isRunning", false);
                }
            }
            else
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isWalking", false);
            }
            if (transform.eulerAngles.y != targetAngle)
            {
                //ROTATION
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnTime);
                transform.rotation = Quaternion.Euler(0, angle, 0);
            }
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
        }
        



        desiredMovement.y = yVel;


        characterController.Move(desiredMovement * Time.deltaTime);


        //GRAVITY
        grounded = characterController.isGrounded;

        if (grounded)
        {
            desiredMovement.y = groundedGravity;
        }
        else
        {
            desiredMovement.y += gravity * Time.deltaTime;
        }

        //JUMPING CD

        if (!jumping)
        {
            if (!canJump && jumpCDCounter >= jumpCD)
            {
                canJump = true;
            }
            else if (!canJump && jumpCDCounter < jumpCD)
            {
                jumpCDCounter += Time.deltaTime;
            }
        }
        else if (grounded)
        {
            jumpCDCounter = 0;
            jumping = false;
            animator.SetBool("isJumping", false);
        }

        //DOGING CD
        if (!dodging)
        {
            if (!canDodge)
            {
                if (dodgeCDCounter >= dodgeCD)
                {
                    canDodge = true;
                    dodgeCDCounter = 0;
                }
                else if (dodgeCDCounter < dodgeCD)
                {
                    dodgeCDCounter += Time.deltaTime;
                }
            }
            
        }
    }

    


    void OnEnable()
    {
        playerInput.PlayerControls.Enable();
    }
    void OnDisable()
    {
        playerInput.PlayerControls.Disable();
    }

}
