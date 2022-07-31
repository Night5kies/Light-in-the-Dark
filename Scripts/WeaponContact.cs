using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponContact : MonoBehaviour
{
    CharacterController characterController;
    PlayerInput playerInput;
    Animator animator;
    Animator enemyState;

    public float damage;
    
    bool leftClick;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); 

        playerInput.PlayerControls.Attack.started += inp => { leftClick = inp.ReadValueAsButton(); };
        playerInput.PlayerControls.Attack.canceled += inp => { leftClick = inp.ReadValueAsButton(); };
    }

    // Update is called once per frame
    void OnColliderEnter(Collision collision)
    {
        if (collision.gameObject.tag == "enemy" && animator.GetBool("isAttacking")){
            EntityHealth enemyScript = collision.gameObject.GetComponent<EntityHealth>();
            enemyState = collision.gameObject.GetComponent<Animator>();
            if (!enemyState.GetBool("isBlocking"))
            {
                enemyScript.changeHealth(0 - damage);
            }
        }
    }
}
