using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class WartHogScript : MonoBehaviour
{
    [Tooltip("If the agent has been trained")]
    private bool tamed = false;

    [Tooltip("Hovering tooltip script that controls UI of WartHog")]
    HoveringTooltip hoveringScript;

    [Tooltip("Hovering tooltipt script that controls tamed ui of warthog")]
    TamedScript tamedScript;

    [Tooltip("The text field showing amount tamed")]
    public TMP_Text tamingPercent;

    [Tooltip("The point behind the player that warthog should follow")]
    public GameObject followPointPlayer;

    // How close warthog is to being tamed
    private float amntTamed;

    //Wander radius
    private float wanderRadius;

    //This agents navMesh
    private NavMeshAgent agent;

    //This agents animator
    private Animator anim;

    private Vector3 lastPosition;

    // Starting health of warthog
    private float health = 50f;

    // Current health of warthog
    private float currentHealth;

    // Health of dead body
    private float deathHealth = 50f;

    //if warthog is dead
    private bool alreadyDead = false;

    // amount of narco in system
    private float amntNarcotics;

    // Amount of narco it takes to fall to sleep
    private float narcoLimit;

    // Controls whether warthog follows or not based on its modulus by 2
    private float followOrNot = 1;

    // if the boar is sleeping
    private bool sleeping = false;

    // Start is called before the first frame update
    void Start()
    {
        //Setting agent to this objects agent 
        agent = GetComponent<NavMeshAgent>();

        // Setting anim to this objects animator
        anim = GetComponent<Animator>();

        //How far the next wander point will be placed
        wanderRadius = Random.Range(10f, 177f);

        // Settign CurrentHealth to health
        currentHealth = health;

        // Setting narcoLimit
        narcoLimit = 3f;

        amntNarcotics = 0f;

        // Setting hoveringScript to this wartHogs hoveringTooltip Script
        hoveringScript = gameObject.GetComponent<HoveringTooltip>();
        hoveringScript.enabled = false;

        // Setting tamedScript to be this wartHogs tamedScript
        tamedScript = gameObject.GetComponent<TamedScript>();
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        _ = Vector3.Distance(transform.position, RandomVector3SpherePosition());

        float speed = Vector3.Distance(lastPosition, gameObject.transform.position) * 100f;
        lastPosition = transform.position;
        if (agent.isActiveAndEnabled)
        {
            if (tamed == false && agent.remainingDistance <= agent.stoppingDistance)
            {
                Seek(RandomVector3SpherePosition());
            }
            if (tamed == true)
            {
                if (followOrNot % 2 == 0)
                {
                    Seek(followPointPlayer.transform.position * -7f);
                } else if (followOrNot % 2 != 0)
                {
                    agent.Stop();
                }
            }

        }
        anim.SetFloat("Speed", speed);

        if (alreadyDead == false && currentHealth <= 0)
        {
           StartCoroutine(Die());
        }
        
        if (sleeping == false && amntNarcotics > narcoLimit)
        {
            StartCoroutine(Sleep());
        }

        // if the warthog is tamed 
        if (amntTamed == 100)
        {
            tamed = true;
            tamedScript.Tamed(true);
            hoveringScript.enabled = false;
        }
    }

    /// <summary>
    /// Increase the amount of narcos in objects system
    /// </summary>
    /// <param name="narcoAmnt">The amount of narco</param>
    public void IncreaseNarcos(float narcoAmnt)
    {
        amntNarcotics += narcoAmnt;
    }


    public Vector3 RandomVector3SpherePosition()
    {
        Vector3 target = Vector3.zero;
        Vector3 randVect = Random.insideUnitSphere * wanderRadius;

        randVect += transform.position;

        if (NavMesh.SamplePosition(randVect, out NavMeshHit hit, wanderRadius, -1))
        {
            target = hit.position;
        }
        return target;
    }


    private void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }


    /// <summary>
    /// Called when a collider hits this objects collider
    /// </summary>
    /// <param name="collision"> The collider hitting this objects collider</param>
    private void OnTriggerEnter(Collider collision)
    {
        // if the players sword hits this objects collider
        if (collision.CompareTag("PlayerSword"))
        {
            //Decrease warthogs health
            currentHealth -= 15f;

        }

        if (alreadyDead == true && collision.CompareTag("PlayerSword"))
        {
            gameObject.tag = "DeadWarthog";
            deathHealth -= 15f;

            if (deathHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }


    /// <summary>
    /// Death mechanics of warthog
    /// </summary>
    private IEnumerator Die()
    {
        alreadyDead = true;
        anim.SetTrigger("Dead");
        agent.enabled = false;

        yield return new WaitForSeconds(120f);

        Destroy(gameObject);
    }

    public IEnumerator Sleep()
    {
        sleeping = true;
        anim.SetBool("Sleep", true);
        agent.enabled = false;
        hoveringScript.enabled = true;
        yield return new WaitForSeconds(60f * (amntNarcotics - narcoLimit));
        anim.SetBool("Sleep", false);
        amntNarcotics = 0;
        if (tamed == false)
        {
            
        }
        agent.enabled = true;
        hoveringScript.enabled = false;
        sleeping = false;
    }

    /// <summary>
    /// We will create tame mechanics in this function
    /// </summary>
    public void Tame()
    {
        Debug.Log("Entered Tame Class");
        // if warthog is sleeping we can feed it 
        if (sleeping /*&& Harvesting.amntOfBerries >= 1*/ && amntTamed < 100)
        {
            Debug.Log("Entered If Statement Tamed Class");
            Harvesting.amntOfBerries -= 1;
            amntTamed += Harvesting.amntOfBerries * 10;
            if (amntTamed > 100)
            {
                amntTamed = 100;
            }
            tamingPercent.text = amntTamed.ToString();
            
        }
    }


    /// <summary>
    /// After Warthog is tamed toggle follow on and off
    /// </summary>
    public void Follow()
    {
        followOrNot++;
    }
}
