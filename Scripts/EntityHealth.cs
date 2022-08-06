using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EntityHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;

    public GameObject player;
    Animator animator;

    public bool hittable = true;
    public float hitSpacing = 0.5f;
    public float hitCounter = 0;

    public TextMeshProUGUI text;
    EnemyController enemyController;


    private void Start()
    {
        resetHealth();
        animator = player.GetComponent<Animator>();
        enemyController = GetComponent<EnemyController>();
    }


    private void Update()
    {
        checkAlive();
        text.text = health.ToString();
        if (!hittable && !animator.GetCurrentAnimatorStateInfo(0).IsName("Sword Slash"))
        {
            hittable = true;
            /*hitCounter += Time.deltaTime;
            if (hitCounter >= hitSpacing)
            {
                hittable = true;
                hitCounter = 0;
            }*/
        }
    }


    public void resetHealth()
    {
        health = maxHealth;
    }

    public void checkAlive()
    {
        if (health <= 0)
        {
            Debug.Log("ENEMY DIED");
            enemyController.enabled = false;
            this.enabled = false;
        }
    }


    public float getHealth()
    { return health; }

    public float setHealth ( float a )
    { return health = a; }

    public float Damage( float change)
    {
        return health -= change;
    }
}
