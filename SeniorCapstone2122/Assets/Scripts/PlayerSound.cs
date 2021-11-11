using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSound : MonoBehaviour
{
    private Controls controls;
    private PlayerMovement movementScript;
    public float percentWalking;
    Vector3 velocity;
    float multiplier;
    public GameObject[] enemies;
    bool sprint;
    // Start is called before the first frame update
    void Start()
    {
        controls = new Controls(); //set the controls variable to our Input Action
        controls.Enable(); //enable the Action Maps
        movementScript = GetComponent<PlayerMovement>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        velocity = movementScript.movementVector;
        if (velocity.x == 0 && velocity.z == 0 && !movementScript.jump)
            multiplier = 0.2f;
        else if (velocity.x != 0 && velocity.z != 0 && movementScript.grounded && controls.Gameplay.Sprint.ReadValue<float>() == 0)
            multiplier = 0.8f;
        else if (velocity.x != 0 && velocity.z != 0 && movementScript.grounded && controls.Gameplay.Sprint.ReadValue<float>() != 0)
            multiplier = 1f;
        else multiplier = 1.2f;
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<Enemy>().listeningMultiplier = multiplier;
        }
    }
}
