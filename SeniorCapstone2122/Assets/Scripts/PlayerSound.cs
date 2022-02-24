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
        audioSource = GetComponent<AudioSource>(); //set audiosource to component on player
        controls = new Controls(); //set the controls variable to our Input Action
        controls.Enable(); //enable the Action Maps
        movementScript = GetComponent<PlayerMovement>(); 
        enemies = GameObject.FindGameObjectsWithTag("Enemy"); //put every enemy in scene in array
    }

    // Update is called once per frame
    void Update()
    {
        sprint = (controls.Gameplay.Sprint.ReadValue<float>() != 0); //get sprinting input
    }

    void LateUpdate()
    {
        velocity = movementScript.movementVector; //get current player input
        if (velocity.x == 0 && velocity.z == 0 && !movementScript.jump) //runs for no movement
            {
                multiplier = 0.2f; //set enemies hearing range multiplier to 20%
                if (playing) //check if any audio is currently playing
                {
                    audioSource.Stop(); //stop current audio
                    playing = false; //set playing to false
                }
            }
        else if (velocity.x != 0 && velocity.z != 0 && movementScript.grounded && !sprint) //runs for movement without sprint
            {
                multiplier = 0.8f; //set enemies hearing range multiplier to 80%
                audioSource.loop = true; //set audio source to loop until stopped
                audioSource.clip = walkSound; //set audio source sound to walk cycle
                if (playing) audioSource.Stop(); //stop current sound if playing
                else playing = true; //otherwise set playing to true
                audioSource.Play(); //start playing audio clip
            }
        else if ((velocity.x != 0 || velocity.z != 0) && movementScript.grounded && sprint) //runs for sprinting movement
            {
                multiplier = 1f; //set enemies hearing range multiplier to 100%
                audioSource.loop = true; //set audio to loop until stopped
                audioSource.clip = runSound; //set audio clip to run cycle
                if (playing) audioSource.Stop(); //stop current sound if playing
                else playing = true; //otherwise set playing to true
                audioSource.Play(); //start playing audio clip
            }
        else //runs for airborne stuff
        {
            multiplier = 1.2f; //set enemies hearing range multiplier to 120%
            audioSource.loop = false; //don't loop
            audioSource.clip = fallSound; //set noise to landing
            if (playing) audioSource.Stop(); //stop current sound if playing
            else playing = true; //otherwise set playing to true
            audioSource.Play(); //start playing audio clip
        }
        foreach (GameObject enemy in enemies) //runs once for each enemy in array
        {
            enemy.GetComponent<Enemy>().listeningMultiplier = multiplier; //set enemies listening multipler to stored one
        }
    }
}
