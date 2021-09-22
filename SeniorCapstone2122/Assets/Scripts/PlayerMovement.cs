using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController playerCC;
    public float movementSpeed;
    private Vector2 inputVector;
    private Vector3 movementVector;
    public float sprintMult;
    // Start is called before the first frame update
    void Start()
    {
        playerCC = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        movementVector.Set(inputVector.x, 0, inputVector.y);
    }

    void FixedUpdate()
    {
        //if (playerCC.isGrounded)
        //{
            playerCC.Move(movementVector * movementSpeed * Time.fixedDeltaTime);
        //}
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputVector = context.action.ReadValue<Vector2>();
    }
}
