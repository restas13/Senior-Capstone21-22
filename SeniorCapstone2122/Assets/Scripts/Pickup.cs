using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public bool isAmmo; //act as health pickup if false

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (isAmmo) other.gameObject.SendMessage("Refill");
            else other.gameObject.SendMessage("Heal");
            gameObject.SetActive(false);
        } else if (other.tag == "Enemy") gameObject.SetActive(false);
    }
}
