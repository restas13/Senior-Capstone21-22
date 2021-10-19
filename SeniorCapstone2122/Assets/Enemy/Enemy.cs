using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private GameObject player;
    NavMeshAgent enemyNavMeshAgent;

    [Header("Enemy Stats")]
    public float speed = 5f;
    public float enemyFieldOfView = 40f;
    public float detectionRadius = 3f;
    public float listeningRadius = 30f;
    public float listeningMultiplier = 1f;
    public float hearingThreshold = 5f;
    [Header("")]
    public bool hasWaypoints;
    public bool seePlayer;
    public bool detectedPlayer;
    public bool withinReach;
    [Header("")]
    public GameObject[] waypoints;
    public Vector3 targetDir;
    Vector3 midBodyPosition;
    Vector3 closestWaypoint;
    [Header("")]
    public float angleFOV;
    public float distanceToPlayer;
    [Header("")]
    public LayerMask playerMask;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerMask = LayerMask.GetMask("Player");
        enemyNavMeshAgent = GetComponent<NavMeshAgent>();
        waypoints = GameObject.FindGameObjectsWithTag("EnemyWaypoint");
    }

    void FixedUpdate()
    {
        
        //Checking if there is Waypoints
        if(waypoints != null)
        {
            hasWaypoints = true;
        }

        midBodyPosition = transform.position + new Vector3(0f, 1.5f, 0f); //Middle Position of Enemy model
        distanceToPlayer = Vector3.Distance(player.transform.position, midBodyPosition); //Distance to player

        targetDir = player.transform.position - midBodyPosition; //Direction towards player
        angleFOV = Vector3.Angle(targetDir, transform.forward); //Angle from Enemy line of sight to player

        if(Mathf.Abs(angleFOV) < enemyFieldOfView / 2) //If within enemy FOV
        {
            withinSightRange(); //Checks if enemy can see player collider
        }else{
            seePlayer = false;
        }
        
    }

    void Update()
    {

        if(seePlayer == true){
            detectedPlayer = true; //If enemy can see player, player is detected
            Debug.Log("1");
        } else if(withinReach == true){
            detectedPlayer = true; //If enemy is within melee range, player is detected
            Debug.Log("2");
        } else {
            Debug.Log("3");
            detectedPlayer = false; //Enemy has met no requirements to see player
        }
    }

    public void Listening()
    {
        if(distanceToPlayer <= (listeningRadius * listeningMultiplier)){

        }
    }

    public void withinSightRange()
    {       
        RaycastHit hit;
        Physics.Raycast(midBodyPosition, targetDir, out hit, Mathf.Infinity);

        if(hit.collider.gameObject == player)
        {
            seePlayer = true;
            Debug.DrawRay(midBodyPosition, targetDir, Color.red);
            return;
        }
        Debug.DrawRay(midBodyPosition, targetDir, Color.blue);

    }
    
    public void HitDetectorSphere() //Check if player is within melee range
    {
        if(distanceToPlayer <= detectionRadius)
        {
            withinReach = true;
        } else
        {
            withinReach = false;
        }
        
    }
    
    public void Wander()
    {
        float closestDistance = 999999999f;
        foreach(GameObject waypoint in waypoints)
        {
            if(Vector3.Distance(transform.position, waypoint.transform.position) < closestDistance)
            {
                closestWaypoint = waypoint.transform.position;
                closestDistance = Vector3.Distance(transform.position, waypoint.transform.position);
            }
        }

        if(closestWaypoint != null){
            enemyNavMeshAgent.SetDestination(closestWaypoint);
        }
    }

    public void Chase() 
    {
        enemyNavMeshAgent.SetDestination(player.transform.position);
    }

    void OnDrawGizmos()
    {
        float rayRange = 10.0f;
        float halfFOV = enemyFieldOfView / 2.0f;

        //line of sight
        Gizmos.color = Color.red;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Gizmos.DrawRay(transform.position + new Vector3(0f, 1.5f, 0f), leftRayDirection * rayRange );
        Gizmos.DrawRay(transform.position + new Vector3(0f, 1.5f, 0f), rightRayDirection * rayRange );
        
        //detection sphere
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0f, 1.5f, 0f), detectionRadius);

        //listening sphere
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0f, 1.5f, 0f), listeningRadius);

        //player tracker
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, targetDir);
    }
}
