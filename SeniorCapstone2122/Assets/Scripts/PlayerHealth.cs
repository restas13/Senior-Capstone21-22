using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    private int health;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Die()
    {
        //death stuff
    }

    public void TakeDamage(int damage)
    {
        health -= 1;
        if (health <= 0)
            Die();
        //do stuff like effects and whatever here
    }
}
