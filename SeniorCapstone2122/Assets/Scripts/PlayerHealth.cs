using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 2;
    private int health;
    private float fastSpeed;
    private float slowSpeed;
    private float baseSpeed;
    private PlayerMovement movement;
    private float regenTime;
    private Vector3 checkpointLocation;
    private GameObject[] pickups;
    public float timeToRegen = 20f;
    GameObject deathUI;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth; //set current health to whatever max health when starting
        checkpointLocation = transform.position;
        movement = GetComponent<PlayerMovement>();
        baseSpeed = movement.movementSpeed;
        slowSpeed = movement.movementSpeed / 2;
        fastSpeed = movement.movementSpeed * 10;
        pickups = GameObject.FindGameObjectsWithTag("Pickup");
        deathUI = GameObject.Find("DeathBackground");
        deathUI.SetActive(false);
    }

    void LateUpdate() 
    {
        if (health < maxHealth && regenTime < Time.time)
        {
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
        Cursor.lockState = CursorLockMode.None;
        deathUI.SetActive(true);
        movement.isDead = true;
        transform.position = checkpointLocation;
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

    private IEnumerator DamageSpeed()
    {
        movement.canSprint = false;
        movement.movementSpeed = fastSpeed;
        yield return new WaitForSeconds(0.1f);
        movement.movementSpeed = slowSpeed;
        yield return new WaitForSeconds(5f);
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

    public void Respawn()
    {
        Cursor.lockState = CursorLockMode.Locked;
        health = maxHealth;
        deathUI.SetActive(false);
        movement.isDead = false;
    }
}
