using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private GameObject player;
    private GameObject closestWaypointToPlayer;
    NavMeshAgent enemyNavMeshAgent;
    public Animator animator;

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
    private bool withinReach;
    bool lookingForWaypoint;
    [Header("")]
    private GameObject waypointPrefab;
    private GameObject[] waypoints;
    public Vector3 targetDir;
    Vector3 midBodyPosition;
    GameObject closestWaypoint;
    [Header("")]
    public float angleFOV;
    public float distanceToPlayer;
    private bool stunned;
	private float attackCD;


    void Awake(){
        player = GameObject.FindWithTag("Player");
        enemyNavMeshAgent = GetComponent<NavMeshAgent>();
        waypoints = GameObject.FindGameObjectsWithTag("EnemyWaypoint");
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        waypoints = GameObject.FindGameObjectsWithTag("EnemyWaypoint");
        //GameObject.Instantiate
    }

    void FixedUpdate()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Sword And Shield Attack")){
            //Attack();
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
        
        //Checking if there is Waypoints
        if(waypoints != null)
        {   
            if(animator.GetBool("attack") == true || animator.GetBool("chase") == true){
                
                return;
            }
            hasWaypoints = true;
        }

    }

    void Update()
    {
        if(!stunned)
        {
        if(seePlayer == true){
            detectedPlayer = true; //If enemy can see player, player is detected
            Debug.Log("1");
            animator.SetBool("chase", true);
        } else if(withinReach == true){
            detectedPlayer = true; //If enemy is within melee range, player is detected
            Debug.Log("2");
            animator.SetBool("chase", true);
        } else {
            Debug.Log("3");
            detectedPlayer = false; //Enemy has met no requirements to see player
            animator.SetBool("chase", false);
        }
        } else {
            animator.SetBool("chase", false);
        }
    }


#region Detection
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
            animator.SetTrigger("attack");
        } else
        {
            withinReach = false;
        }
        
    }
#endregion
#region Enemy States
    public void Wander()
    {

        if (closestWaypoint != null)
        {
            if(transform.position == closestWaypoint.transform.position){
                lookingForWaypoint = true;
            }
        } else 
        {
            lookingForWaypoint = true;
        }
        if(lookingForWaypoint == true)
        {
            foreach(GameObject waypoint in waypoints)
            {
                if(closestWaypoint == null){
                    closestWaypoint = waypoint;
                }
                else if(Vector3.Distance(transform.position, waypoint.transform.position) < Vector3.Distance(closestWaypoint.transform.position, transform.position))
                {
                    closestWaypoint = waypoint;
                }
            }
            lookingForWaypoint = false;
        } else {
            //do nothing
        }
        
        if(closestWaypoint != null){
            enemyNavMeshAgent.SetDestination(closestWaypoint.transform.position);
        }
    }
    public void LookforPlayer(){
        foreach(GameObject waypoint in waypoints){
            if(Vector3.Distance(closestWaypointToPlayer.transform.position, player.transform.position) > (Vector3.Distance(waypoint.transform.position, player.transform.position))){
                closestWaypointToPlayer = waypoint;
            }
        }
        
    }
    public void Chase() 
    {
        enemyNavMeshAgent.SetDestination(player.transform.position);
    }

    public void Attack(){
		if (Time.time > attackCD)
		{
        	enemyNavMeshAgent.SetDestination(transform.position);
			player.GetComponent<PlayerHealth>().TakeDamage(1);
			attackCD = Time.time + 10f;
		}
    }

    public void TakeDamage()
    {
        StartCoroutine("Stunned");
        enemyNavMeshAgent.ResetPath();
    }
    private IEnumerator Stunned()
    {
        animator.SetBool("stunned", true);
        stunned = true;
        yield return new WaitForSeconds(5f);
        stunned = false;
        animator.SetBool("stunned", false);
    }
#endregion
#region Gizmos
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
#endregion