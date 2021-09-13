using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController playerCC;
    public float movementSpeed;

    private Vector3 movementVector;
    // Start is called before the first frame update
    void Start()
    {
        playerCC = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        movementVector = transform.forward * Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;
        movementVector += transform.right * Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;
    }

    void FixedUpdate()
    {
        if (playerCC.isGrounded)
        {
            playerCC.Move(movementVector);
        }
    }
}
