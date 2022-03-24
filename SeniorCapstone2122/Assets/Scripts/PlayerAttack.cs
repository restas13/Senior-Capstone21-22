using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private Controls controls;
    public int maxShots;
    public int currentShots;
    public float rechargeTime;
    private float finishChargeTime;
    public bool aim;
    public GameObject handObject;
    private LineRenderer lineRenderer;
    private bool toggle = true;
    // Start is called before the first frame update
    void Start()
    {
        controls = new Controls();
        controls.Enable();
        lineRenderer = handObject.GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        aim = Mouse.current.rightButton.isPressed;
        if(aim)
        {
            handObject.SetActive(true);
            toggle = true;
            //show hand and stuff
            if(Mouse.current.leftButton.wasPressedThisFrame && currentShots > 0)
            {
                Fire();
            }
        } else if (toggle)
        {
            handObject.SetActive(false);
            toggle = false;
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
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out raycastHit);
        lineRenderer.SetPosition(0, handObject.transform.position);
        if(raycastHit.collider != null)
        {
            lineRenderer.SetPosition(1, raycastHit.point);
            if(raycastHit.collider.tag == "Enemy")
            {
                raycastHit.collider.gameObject.SendMessage("TakeDamage");
            }
        } else 
        {
            lineRenderer.SetPosition(1, Camera.main.transform.position + (Camera.main.transform.TransformDirection(Vector3.forward) * 100f));
        }
        StartCoroutine("ShotEffect");
        currentShots -= 1;
        finishChargeTime = Time.time + rechargeTime;
    }

    public void Refill()
    {
        currentShots = maxShots;
    }

    private IEnumerator ShotEffect()
    {
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.7f);
        lineRenderer.enabled = false;
    }
}
