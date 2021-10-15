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
    // Start is called before the first frame update
    void Start()
    {
        controls = new Controls(); //set the controls variable to our Input Action
        controls.Enable(); //enable the Action Maps
        movementScript = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        velocity = movementScript.playerVelocity;
        
    }
}
