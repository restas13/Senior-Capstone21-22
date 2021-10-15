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
    private Vector3 movementVector;
    public Vector3 playerVelocity;
    private Vector3 camRotation;
    private Vector2 mouseInput;
    private bool jump;
    public float lookSensitivity;
    public float movementSpeed;
    public float sprintMult;
    public float jumpHeight = 1.5f;

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
        movementVector = (transform.forward * inputVector.y) + (transform.right * inputVector.x); //set vector for movement
        mouseInput.Set(Mouse.current.delta.x.ReadValue(), Mouse.current.delta.y.ReadValue() * -1); //read current mouse position change
        if (playerCC.isGrounded == true && !jump && controls.Gameplay.Jump.triggered) //save jump input for next fixed update
            jump = true;
        transform.Rotate(new Vector3(0, mouseInput.x, 0) * lookSensitivity, Space.Self); //rotate player horizontally with mouse
        if(controls.Gameplay.Pause.triggered)
        {
            Cursor.lockState = CursorLockMode.None; //unlock cursor if pause button is pressed
        }
        //there is DEFINITELY a better way to do everything below
        camRotation.Set(camRotation.x, mainCam.transform.rotation.eulerAngles.y, mainCam.transform.rotation.eulerAngles.z); //keep current rotation of camera in y and z axes
        camRotation.x += mouseInput.y * lookSensitivity; //use mouse input to rotate camera
        if (camRotation.x < 90 && camRotation.x > -90) //keep player from looking too far up or down
            mainCam.transform.rotation = Quaternion.Euler(camRotation); //actually rotate
        else if(camRotation.x >= 90) //fix if somehow rotated too far
        {
            camRotation.Set(89f, 0, 0); //fix rotation
            mainCam.transform.rotation = Quaternion.Euler(camRotation); //actually rotate
        }
        else if(camRotation.x <= -90) //fix if somehow rotated too far
        {
            camRotation.Set(-89f, 0, 0); //fix rotation
            mainCam.transform.rotation = Quaternion.Euler(camRotation); //actually rotate
        }
    }

    void FixedUpdate()
    {
        if (playerCC.isGrounded) //get if grounded so we can apply gravity if airborne
        {
            if (controls.Gameplay.Sprint.ReadValue<float>() != 0) //get if sprint button is pressed
                movementVector *= sprintMult; //apply sprinting multiplier
            playerCC.Move(movementVector * movementSpeed * Time.fixedDeltaTime); //do the forward/leftright movement
            if (jump)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3f * Physics.gravity.y); //add jump height to velocity
                jump = false;
            }
        } else
        {
            playerVelocity.y += Physics.gravity.y * Time.fixedDeltaTime; //subtract gravity
        }
        if (playerVelocity.y < 0 && playerCC.isGrounded) //dont give y velocity if grounded and not jumping
            playerVelocity.y = 0f;
        playerCC.Move(playerVelocity * Time.fixedDeltaTime); //move
    }
}