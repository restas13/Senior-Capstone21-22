using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 2;
    public int health;
    private float fastSpeed;
    private float slowSpeed;
    private float baseSpeed;
    private PlayerMovement movement;
    private float regenTime;
    private Vector3 checkpointLocation;
    private GameObject[] pickups;
    public float timeToRegen = 20f;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        movement = GetComponent<PlayerMovement>();
        baseSpeed = movement.movementSpeed;
        slowSpeed = movement.movementSpeed / 2;
        fastSpeed = movement.movementSpeed * 10;
        pickups = GameObject.FindGameObjectsWithTag("Pickup");
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.fKey.wasReleasedThisFrame){
            
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
        if(pickups.Length > 0)
        {
            foreach(GameObject pickup in pickups)
            {
                pickup.SetActive(true);
            }
        }
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

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Checkpoint")
        {
            checkpointLocation = collider.gameObject.transform.position;
        }
    }

    public void Heal()
    {
        health = maxHealth;
    }
}
