using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public float turnSpeed;
    float turnSmoothVelocity;
    float targetAngle;
    public float turnTime;

    float yVel;

    bool movementInputGiven;

    bool grounded;
    bool running;
    bool jumpPressed;
    bool jumping;

    Vector2 movementInput;
    Vector3 desiredMovement;


    


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

        playerInput.PlayerControls.Jump.started += inp => { jumpPressed = inp.ReadValueAsButton(); };
        playerInput.PlayerControls.Jump.canceled += inp => { jumpPressed = inp.ReadValueAsButton(); };

    }

    // handler function to set the player input values
    void AssignMovementInput(InputAction.CallbackContext inp)
    {
        movementInput = inp.ReadValue<Vector2>();
        movementInputGiven = movementInput.x != 0f || movementInput.y != 0f;
    }



    void Update()
    {
        desiredMovement.x = 0;
        desiredMovement.z = 0;
        yVel = desiredMovement.y;
        

        //MOVEMENT
        if (movementInputGiven)
        {
            targetAngle = Mathf.Atan2(movementInput.x, movementInput.y) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;

            if (running)
            {
                desiredMovement = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward * runSpeed;
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);
            }
            else
            {
                desiredMovement = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward * walkSpeed;
                animator.SetBool("isRunning", false);
                animator.SetBool("isWalking", true);
            }

            //turnTime = Mathf.Abs(targetAngle - transform.eulerAngles.y) / turnSpeed;
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
        }
        desiredMovement.y = yVel;

        //GRAVITY
        

        characterController.Move(desiredMovement * Time.deltaTime);
        grounded = characterController.isGrounded;

        if (grounded)
        {
            desiredMovement.y = groundedGravity;
        }
        else
        {
            desiredMovement.y += gravity * Time.deltaTime;
        }

        //JUMPING
        if (jumpPressed && !jumping && grounded)
        {
            jumping = true;
            desiredMovement.y = jumpVel;
            animator.SetBool("isJumping", true);
        }
        else if (grounded && jumping)
        {
            jumping = false;
            animator.SetBool("isJumping", false);
        }
        

        //ROTATION
        if (transform.eulerAngles.y != targetAngle)
        {
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);
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
