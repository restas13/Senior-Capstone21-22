using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 2;
    private int health;
    private PlayerMovement movement;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)){
            TakeDamage(1);
        }
    }

    void Die()
    {
        //death stuff
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            Die();
        //do stuff like effects and whatever here
    }

    private IEnumerator DamageSpeed(){
        movement.canSprint = false;
        movement.movementSpeed *= 10;
        yield return new WaitForSeconds(0.1f);
        movement.movementSpeed /= 10;
        movement.movementSpeed /= 2;
        yield return new WaitForSeconds(7f);
        movement.movementSpeed *= 2;
        movement.canSprint = true;
    }
}
