using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public bool isAmmo; //act as health pickup if false

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") //check if colliding object is player
        {
            if (isAmmo) other.gameObject.SendMessage("Refill"); //refill players ammo if this is an ammo pickup
            else other.gameObject.SendMessage("Heal"); //heal otherwise
            gameObject.SetActive(false); //disable self
        } else if (other.tag == "Enemy") gameObject.SetActive(false); //just disable self if enemy collides
    }
}
