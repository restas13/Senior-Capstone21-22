using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 2;
    private int health;
    private float fastSpeed;
    private float slowSpeed;
    private float baseSpeed;
    private PlayerMovement movement;
    private float regenTime;
    public float timeToRegen = 20f;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        movement = GetComponent<PlayerMovement>();
        baseSpeed = movement.movementSpeed;
        slowSpeed = movement.movementSpeed / 2;
        fastSpeed = movement.movementSpeed * 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.fKey.isPressed){
            TakeDamage(1);
        }
    }

    void FixedUpdate() 
    {
        if (health < maxHealth && regenTime >= Time.time){
            health = maxHealth;
        }
    }

    private void Die()
    {
        //death stuff
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            Die();
        if(!movement.canSprint) {
            StopCoroutine(DamageSpeed());
            movement.movementSpeed = 6f;
            movement.canSprint = true;
        }
        StartCoroutine("DamageSpeed");
        regenTime = Time.time + timeToRegen;
    }

    private IEnumerator DamageSpeed(){
        movement.canSprint = false;
        movement.movementSpeed = fastSpeed;
        yield return new WaitForSeconds(0.1f);
        movement.movementSpeed = slowSpeed;
        yield return new WaitForSeconds(7f);
        movement.movementSpeed = baseSpeed;
        movement.canSprint = true;
    }
}
