using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSound : MonoBehaviour
{
    private AudioSource audioSource;
    private Controls controls;
    private PlayerMovement movementScript;
    private float percentWalking;
    private Vector3 velocity;
    private float multiplier;
    private GameObject[] enemies;
    public bool sprint;
    private bool playing;
    public AudioClip walkSound;
    public AudioClip runSound;
    public AudioClip fallSound;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        controls = new Controls(); //set the controls variable to our Input Action
        controls.Enable(); //enable the Action Maps
        movementScript = GetComponent<PlayerMovement>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        sprint = (controls.Gameplay.Sprint.ReadValue<float>() != 0);
    }

    void LateUpdate()
    {
        velocity = movementScript.movementVector;
        if (velocity.x == 0 && velocity.z == 0 && !movementScript.jump)
            {
                multiplier = 0.2f;
                if (playing)
                {
                    audioSource.loop = true;
                    audioSource.Stop();
                    playing = false;
                }
            }
        else if (velocity.x != 0 && velocity.z != 0 && movementScript.grounded && !sprint)
            {
                multiplier = 0.8f;
                if (!playing)
                {
                    audioSource.loop = true;
                    audioSource.clip = walkSound;
                    audioSource.Play();
                    playing = true;
                } else {
                    audioSource.clip = walkSound;
                    audioSource.Stop();
                    audioSource.Play();
                }
            }
        else if (velocity.x != 0 && velocity.z != 0 && movementScript.grounded && sprint)
            {
                multiplier = 1f;
                if (!playing)
                {
                    audioSource.loop = true;
                    audioSource.clip = runSound;
                    audioSource.Play();
                    playing = true;
                } else {
                    audioSource.clip = runSound;
                    audioSource.Stop();
                    audioSource.Play();
                }
            }
        else 
        {
            multiplier = 1.2f;
            if (!playing)
            {
                audioSource.loop = false;
                audioSource.clip = fallSound;
                audioSource.Play();
                playing = true;
            } else {
                audioSource.loop = false;
                audioSource.clip = fallSound;
                audioSource.Stop();
                audioSource.Play();
            }
        }
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<Enemy>().listeningMultiplier = multiplier;
        }
    }
}
