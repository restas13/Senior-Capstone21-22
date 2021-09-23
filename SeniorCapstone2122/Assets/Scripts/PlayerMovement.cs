using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Controls controls;
    private CharacterController playerCC;
    private Vector2 inputVector;
    private Vector3 movementVector;
    public float movementSpeed;
    public float sprintMult;
    // Start is called before the first frame update
    void Start()
    {
        playerCC = GetComponent<CharacterController>();
    }

    void Awake()
    {
        controls = new Controls();
        controls.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        inputVector = controls.Gameplay.Movement.ReadValue<Vector2>();
        movementVector.Set(inputVector.x, 0, inputVector.y);
    }

    void FixedUpdate()
    {
        //if (playerCC.isGrounded)
        //{
            if (controls.Gameplay.Sprint.triggered)
                movementVector *= sprintMult;
            playerCC.Move(movementVector * movementSpeed * Time.fixedDeltaTime);
        //}
    }
}
