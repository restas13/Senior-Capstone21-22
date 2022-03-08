using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; //need this for Input System stuff

public class PlayerMovement : MonoBehaviour
{
    private Controls controls; //variable to contain Input Action
    private CharacterController playerCC;
    private Camera mainCam;
    private Vector2 inputVector;
    public Vector3 movementVector;
    public Vector3 playerVelocity;
    private Vector2 mouseInput;
    public GameObject groundChecker;
    public GameObject frontChecker;
    public bool jump;
    public float lookSensitivity;
    public float movementSpeed;
    public float sprintMult;
    public bool canSprint = true;
    public float jumpHeight = 1.5f;
    public float xRot;
    public bool grounded;
    private Collider[] front;
    public bool isDead;
    public LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        playerCC = GetComponent<CharacterController>();
        mainCam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked; //lock cursor on start
    }

    void Awake()
    {
        controls = new Controls(); //set the controls variable to our Input Action
        controls.Enable(); //enable the Action Maps
    }

    // Update is called once per frame
    void Update()
    {
        inputVector = controls.Gameplay.Movement.ReadValue<Vector2>(); //read value of Vector2 for movement input from Input Action
        mouseInput.Set(Mouse.current.delta.x.ReadValue(), Mouse.current.delta.y.ReadValue() * -1); //read current mouse position change
        if (grounded == true && !jump && controls.Gameplay.Jump.triggered) //save jump input for next fixed update
            jump = true;
        transform.rotation *= Quaternion.Euler(0, mouseInput.x * lookSensitivity, 0); //rotate player horizontally with mouse
        if(controls.Gameplay.Pause.triggered)
        {
            Cursor.lockState = CursorLockMode.None; //unlock cursor if pause button is pressed
        }
        if(!isDead) 
        {
            xRot += mouseInput.y * lookSensitivity; //track total rotation so we can prevent view from being rotated upside down
            if (xRot < 90 && xRot > -90) //only rotate if it will keep us right side up
            {
                mainCam.transform.rotation = Quaternion.Euler(xRot, mainCam.transform.rotation.eulerAngles.y, mainCam.transform.rotation.eulerAngles.z); //rotate
            }
            else if(xRot >= 90) //fix if somehow rotated too far
                xRot = 89; //fix rotation
            else if(xRot <= -90) //fix if somehow rotated too far
                xRot = -89; //fix rotation
        }
    }

    void FixedUpdate()
    {
        grounded = Physics.Raycast(groundChecker.transform.position, Vector3.down, 0.1f); //do a short raycast down from bottom of player to get if grounded
        front = Physics.OverlapSphere(frontChecker.transform.position, 0.74f, mask); //check for non player and ground objects in close proximity to player
        if (grounded) { //only allow player to control movement on ground and so we can apply gravity if airborne
            movementVector = (transform.forward * inputVector.y) + (transform.right * inputVector.x); //set vector for movement
            if (controls.Gameplay.Sprint.ReadValue<float>() != 0 && canSprint && grounded) //get if sprint button is pressed
                movementVector *= sprintMult; //apply sprinting multiplier
            if(jump)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -9f * Physics.gravity.y); //add jump height to velocity
                jump = false; //remove stored jump input
            }
            if (playerVelocity.y < 0) //dont give y velocity if grounded and not jumping
                playerVelocity.y = 0f;
        } else
        {
            playerVelocity.y += Physics.gravity.y * 3 * Time.fixedDeltaTime; //subtract gravity
            if (front.Length != 0) //reverse direction and reduce speed if we'd get stuck on something
                movementVector *= -.1f;
        }
        if(!isDead)
            playerCC.Move(((movementVector * movementSpeed) + playerVelocity) * Time.fixedDeltaTime); //move
    }
}