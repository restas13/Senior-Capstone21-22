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
    private Vector2 mouseInput;
    public float lookSensitivity;
    public float movementSpeed;
    public float sprintMult;
    public Camera mainCam;
    public Vector3 test;
    // Start is called before the first frame update
    void Start()
    {
        playerCC = GetComponent<CharacterController>();
        mainCam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Awake()
    {
        controls = new Controls();
        controls.Enable();
    }

    void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        test = mainCam.transform.rotation.eulerAngles;
        inputVector = controls.Gameplay.Movement.ReadValue<Vector2>();
        //movementVector.Set(inputVector.x, 0, inputVector.y);
        movementVector = (transform.forward * inputVector.y) + (transform.right * inputVector.x);
        mouseInput.Set(Mouse.current.delta.x.ReadValue(), Mouse.current.delta.y.ReadValue() * -1);
        transform.Rotate(new Vector3(0, mouseInput.x, 0) * lookSensitivity * Time.deltaTime, Space.Self);
        //if(mainCam.transform.localRotation.x < 90 && mainCam.transform.localRotation.x > -90)
            mainCam.transform.Rotate(new Vector3(mouseInput.y, 0, 0) * lookSensitivity * Time.deltaTime, Space.Self);
        /*else if(mainCam.transform.rotation.x >= 90)
            mainCam.transform.localRotation = Quaternion.Euler(89f, 0, 0);
        else if(mainCam.transform.rotation.x <= -90)
            mainCam.transform.localRotation = Quaternion.Euler(-89f, 0, 0);*/
        if(controls.Gameplay.Pause.triggered)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void FixedUpdate()
    {
        if (playerCC.isGrounded)
        {
            if (controls.Gameplay.Sprint.ReadValue<float>() != 0)
                movementVector *= sprintMult;
            playerCC.Move(movementVector * movementSpeed * Time.fixedDeltaTime);
        } else 
        {
            playerCC.Move(new Vector3(0, -9.81f, 0) * Time.fixedDeltaTime);
        }
    }
}
