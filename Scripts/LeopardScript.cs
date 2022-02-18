using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeopardScript : MonoBehaviour
{

    [Tooltip("The patrol points")]
    public GameObject[] patrolPoints;

    [Tooltip("The distance from player")]
    private float distanceFromPlayer;

    [Tooltip("The flag gameObjectS")]
    public GameObject player;

    // speed of this bot
    private float speed;

    // This characters animator
    private Animator anim;

    //This characters navmeshAgent
    private NavMeshAgent agent;

    //Is this character already Attacking
    bool alreadyAttacking;

    //last position of this gameObject (used for calculating speed)
    Vector3 lastPosition;

    //The spot in the array we are at
    int currentPatrolNumber = 0;

    /// <summary>
    /// Called before the first frame update
    /// </summary>
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Called once per frame
    /// <see cref="Hide"/ called when bots health is too low>
    /// </summary>
    void Update()
    {


        distanceFromPlayer = Vector3.Distance(gameObject.transform.position, player.transform.position);

        // if health is high enough and distance to player is far enough wander randomly
        if ((distanceFromPlayer > 100f)) { GoToNextPoint(); }

        //// Hide behind an obstacle if health gets too low
        //if (NPCEnemyHealth.currentHealth <= 30f) { view.RPC("Hide", RpcTarget.All); }

        // if healt his high enough and target gets close enough pursue target
        if (distanceFromPlayer <= 100f) { Pursue(); }



    }

    /// <summary>
    /// Sends bot towards chosen location
    /// </summary>
    /// <param name="location">Location bot will travel towards</param>
    void Seek(Vector3 location)
    {
        // Sets destination for NavMeshAgent to follow
        agent.SetDestination(location);
    }

    /// <summary>
    /// Makes this bot patrol accross the points in <see cref="patrolPoints"/>
    /// </summary>
    void GoToNextPoint()
    {
        // current patrol point in patrolPoints array
        GameObject currentPatrolPoint = patrolPoints[currentPatrolNumber];

        //if have not reached current patrol point yet
        if (Vector3.Distance(gameObject.transform.position, currentPatrolPoint.transform.position) > 4f)
        {
            // Move agent towards patrol point
            Seek(currentPatrolPoint.transform.position);
        }
        else // if has reached current patrol point
        {

            // if at the end of the array
            if (currentPatrolNumber == patrolPoints.Length - 1)
            {
                // restart array (we are patrolling in a square)
                currentPatrolNumber = 0;
            }
            else // if not at the end of the array
            {
                // set up next patrol point
                currentPatrolNumber++;
            }
        }
    }

    /// <summary>
    /// Pursue target AKA the player character
    /// </summary>
    void Pursue()
    {
        Seek(player.transform.position);
        //direction of target 
        Vector3 targetDirection = player.transform.position - gameObject.transform.position;

        //// angle this bot is travelling at in relation to target
        //float angle = Vector3.Angle(gameObject.transform.forward, gameObject.transform.TransformVector(targetDirection));

        //// angle from bot to target
        //float toTargetAngle = Vector3.Angle(gameObject.transform.forward, gameObject.transform.TransformVector(targetDirection));

        //// follow if heading in the relative direction of target
        //if ((toTargetAngle > 90 && angle < 20))
        //{
        //    // vector the bot will seek
        //    Vector3 character = player.transform.position + player.transform.position.normalized * 34f;

        //    // Seek character
        //    Seek(character);
        //}
        //else
        //{
        //    Seek(player.transform.position + player.transform.forward * (speed));
        //}
    }

    /// <summary>
    /// Called every 0.2 seconds
    /// </summary>
    private void FixedUpdate()
    {
        // current speed of the bot based on last position and current position * 100
        speed = Vector3.Distance(lastPosition, gameObject.transform.position) * 100f;
        lastPosition = gameObject.transform.position;

        anim.SetFloat("Speed", speed);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (alreadyAttacking = false && collision.collider.tag == "Player")
        {
            StartCoroutine(Attacking());
        }
    }



    private IEnumerator Attacking()
    {
        alreadyAttacking = true;

        PlayerHealth.currentHealth -= 75f;

        yield return new WaitForSeconds(.75f);

        alreadyAttacking = false;
    }
}
