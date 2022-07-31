using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    public float health;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float getHealth()
    { return health; }

    public float setHealth ( float a )
    { return health = a; }

    public float changeHealth ( float change)
    {
        Debug.Log("Damage dealt:" + (-change));
        Debug.Log("Health remaining: " + (health + change));
        return health += change; 
    }
}
