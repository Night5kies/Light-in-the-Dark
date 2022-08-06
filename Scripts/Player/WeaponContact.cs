using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponContact : MonoBehaviour
{
    public GameObject player;

    Animator animator;

    public float damage;
    
    bool leftClick;
    // Start is called before the first frame update
    void Start()
    {        
        animator = player.GetComponent<Animator>(); 
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.tag + animator.GetCurrentAnimatorStateInfo(0).IsName("Sword Slash"));
        if (other.gameObject.tag == "Enemy" && animator.GetCurrentAnimatorStateInfo(0).IsName("Sword Slash"))
        {
            EntityHealth enemyScript = other.gameObject.GetComponent<EntityHealth>();
            //enemyState = other.gameObject.GetComponent<Animator>();
            if (enemyScript.hittable)
            {
                enemyScript.Damage(damage);
                enemyScript.hittable = false;
            }
        }
    }
}
