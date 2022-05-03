using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class WartHogScript : MonoBehaviour
{
    [Tooltip("If the agent has been trained")]
    public bool tamed = false;

    [Tooltip("The text field showing amount tamed")]
    public TMP_Text tamingPercent;

    [Tooltip("The point behind the player that warthog should follow")]
    private GameObject followPointPlayer;

    // How close warthog is to being tamed
    public float amntTamed;

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
    public bool sleeping = false;

    [Tooltip("The tip that is displayed when hovering over sleeping warthog")]
    public string tip;

    // if we are already hovering mouse over sleeping warthog
    private bool alreadyHovering = false;

    // Time before tip 
    private float timeToTip = 0.5f;

    [Tooltip("The sleep UI for warthog")]
    public GameObject sleepingUI;

    // if the sleepUI is already open for this warthog
    private bool sleepUIOpen = false;

    [Tooltip("The tame UI for warthog")]
    public GameObject tamingUI;

    // if the tameUI is already open for this warthog
    private bool tamingUIOpen = false;



    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.name == "CanvasTaming") { return; }
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

        // setting amntNarcotics
        amntNarcotics = 0f;

        // finding the followPointPlayer gameObject with tag "FollowPoint"
        followPointPlayer = GameObject.FindGameObjectWithTag("FollowPoint");
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        // do not perform update mechanics on warthogs canvas. It is a child and only has this script for onMouseOver and other UI interactions 
        if (gameObject.name == "CanvasTaming" || gameObject.name == "CanvasTamed") { return; }
        Debug.Log("This warthog is/is not: " + tamed);

        Debug.Log("Taming percent" + amntTamed);

        // if fed enough to be tamed set tame to true
        if (amntTamed == 100) { tamed = true; }
        _ = Vector3.Distance(transform.position, RandomVector3SpherePosition());

        // Speed of the warthog
        float speed = Vector3.Distance(lastPosition, gameObject.transform.position) * 100f;
        // position of warthog at this frame
        lastPosition = transform.position;
        // if navmesh agent is active
        if (agent.isActiveAndEnabled)
        {
            // if not tamed and not too close to current vector goal
            if (tamed == false && agent.remainingDistance <= agent.stoppingDistance)
            {
                // Seek a random position
                Seek(RandomVector3SpherePosition());
            }
            // if warthog is tamed
            else if (tamed == true)
            {
                // if we have clicked follow button an even amount of times
                if (followOrNot % 2 == 0)
                {
                    // follow behind the player at an invisible sphere
                    Seek(followPointPlayer.transform.position);
                } else if (followOrNot % 2 != 0) // if follow button clicked an uneven amount of times
                {
                    // Stop the agent from moving
                    agent.Stop();
                }
            }

        }
        anim.SetFloat("Speed", speed);

        if (alreadyDead == false && currentHealth <= 0)
        {
           StartCoroutine(Die());
        }
        
        // if not already sleeping and the amount of narcotics in this boar is great enough to put it to sleep
        if (sleeping == false && amntNarcotics > narcoLimit)
        {
            // Start the coroutine sleep to put boar to sleep and let it be tamed 
            StartCoroutine(Sleep());
        }

        // if the warthog is tamed 
        if (amntTamed == 100)
        {
            // Set tamed variable to be true
            tamed = true;
        }
        // set sleepUIOpen to true when active
        if (sleepingUI.activeSelf) { sleepUIOpen = true; }
        // set sleepUIOpen to false when inactive
        if (!sleepingUI.activeSelf) { sleepUIOpen = false; }

        // set tameUIOpen to true when active
        if (tamingUI.activeSelf) { tamingUIOpen = true; }
        // set tameUI to false when inactive
        if (!tamingUI.activeSelf) { tamingUIOpen = false; }
    }

    /// <summary>
    /// Increase the amount of narcos in objects system
    /// </summary>
    /// <param name="narcoAmnt">The amount of narco</param>
    public void IncreaseNarcos(float narcoAmnt)
    {
        if (gameObject.name == "CanvasTaming") { return; }
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
        if (gameObject.name == "CanvasTaming" || gameObject.name == "CanvasTamed") { return; }
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
        // so boar cannot die a second time
        alreadyDead = true;
        // run death animation
        anim.SetTrigger("Dead");
        // disable agent so dead boar does not try to move
        agent.enabled = false;
        // wait for 120 seconds before de instantiation
        yield return new WaitForSeconds(120f);
        //de instantiate
        Destroy(gameObject);
    }

    /// <summary>
    /// Used to control the sleep mechanics of the boar 
    /// </summary>
    /// <returns>an amount of time until boar wakes up</returns>
    public IEnumerator Sleep()
    {
        // Set sleeping to true
        sleeping = true;
        // Start sleeping animation
        anim.SetBool("Sleep", true);
        // disable navmeshagent so does not walk/ run while sleeping
        agent.enabled = false;
        // print a timer up to 60 seconds with console log 
        float timer = 0;
        timer += Time.deltaTime;
        Debug.Log(timer);
        // wait for 60 seconds times the amount of extra narcotics in boars sytem
        yield return new WaitForSeconds(60f);
        // stop sleeping animaion
        anim.SetBool("Sleep", false);
        // set the amount of narcotics in system to zero
        amntNarcotics = 0;
        // re enable agent
        agent.enabled = true;
        // set sleeping to be false so boar can be put to sleep again
        sleeping = false;
    }


    /// <summary>
    /// Called when the mouse is hovering over gameObject
    /// </summary>
    private void OnMouseOver()
    {
        // if warthog is asleep and we are not already hovering mouse over warthog
        if ((sleeping && !alreadyHovering) || (tamed && !alreadyHovering))
        {
            // Stop the coroutine to display tooltip so it is not displayed twice
            StopCoroutine(StartTimerForTooltip());

            // Start the coroutine to display tooltip
            StartCoroutine(StartTimerForTooltip());
        }
        // if sleeping and hovering 
        if (sleeping || tamed)
        {
            // set alreadyHovering to true
            alreadyHovering = true;
        }
    }

    /// <summary>
    /// Called when the mouse stops hovering over gameObject
    /// </summary>
    private void OnMouseExit()
    {
        // Stop coroutine to display tooltip
        StopCoroutine(StartTimerForTooltip());

        // Set tooltip to display to an empty string so we do not get null errors 
        TooltipsManager.OnMouseOver("", Input.mousePosition);

        // Set already hovering to false
        alreadyHovering = false;
    }

    /// <summary>
    /// Used to display a hovering tooltip
    /// </summary>
    private void ShowMessge()
    {
        // Show our preffered tip
        TooltipsManager.OnMouseOver(tip, Input.mousePosition);
    }

    /// <summary>
    /// Starts a half second timer to display tip over gameObject
    /// </summary>
    /// <returns>Returns an amount of time before showing tip</returns>
    private IEnumerator StartTimerForTooltip()
    {
        // Waits .5 seconds
        yield return new WaitForSeconds(timeToTip);

        // Show the message using ShowMessage
        ShowMessge();
    }

    /// <summary>
    /// Displays Sleeping UI if warthog is asleep 
    /// </summary>
    private void OnMouseDown()
    {
        // If not sleeping we do not want to mess with UI's
        if (!sleeping && !tamed) { return; }

        // if sleepUI is not open
        if (!sleepUIOpen && sleeping && !tamed)
        {
            // Set sleepUI active
            sleepingUI.SetActive(true);
        }

        // if tameUI is not open and is not sleeping and is tamed 
        if (!tamingUIOpen && tamed)
        {
            tamingUI.SetActive(true);
        }

        // If sleepingUI is open
        if (sleepUIOpen)
        {
            // Set sleepingUI to inactive
            sleepingUI.SetActive(false);
        }
        // If tamingUI is open 
        if (tamingUIOpen)
        {
            // Set tamingUI to inactive
            tamingUI.SetActive(false);
        }
    }

    /// <summary>
    /// After Warthog is tamed toggle follow on and off
    /// </summary>
    public void Follow()
    {
        followOrNot++;

        if (followOrNot % 2 == 0)
        {
            Debug.Log("Following");
        } else
        {
            Debug.Log("Not Following");
        }
    }

    /// <summary>
    /// Increment taming percent from UI
    /// </summary>
    public void TamingPercentIncrement(int amountIncrease)
    {
        // increment amount tamed by ten
        amntTamed += amountIncrease;
    }
}
