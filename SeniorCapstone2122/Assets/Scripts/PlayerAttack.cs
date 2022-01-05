using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Controls controls;
    public int maxShots;
    private int currentShots;
    public float rechargeTime;
    private float finishChargeTime;
    private bool aim;
    // Start is called before the first frame update
    void Start()
    {
        controls = new Controls();
        controls.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        aim = controls.Gameplay.Aim.triggered;
        if(aim)
        {
            //show hand and stuff
            if(controls.Gameplay.Fire.triggered && currentShots > 0)
            {
                Fire();
            }
        }
    }

    void LateUpdate()
    {
        if(currentShots < maxShots && Time.time > finishChargeTime)
        {
            currentShots = maxShots;
        }
    }

    public void Fire()
    {
        RaycastHit raycastHit;
        Physics.Raycast(Camera.main.transform.position, Vector3.forward, out raycastHit);
        if(raycastHit.collider.tag == "Enemy")
        {
            //do stuff here
        }
        currentShots -= 1;
        finishChargeTime = Time.time + rechargeTime;
    }

    public void Refill()
    {
        currentShots = maxShots;
    }
}
