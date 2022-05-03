using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBoard : MonoBehaviour
{   
    // speed to slide
    public float slidingSpeed;

    // max sliding speed 
    public float maxSlidingSpeed;
    
    // key that initiates slide
    private KeyCode key = KeyCode.F;
    
    // is the player sliding
    private bool isSliding = false;
    // sliding timer
    private float slidingTimer = 0;

    // players rigid body
    private Rigidbody rb;

    /// <summary>
    /// Start is called befor the first frame update
    /// </summary>
    private void Start()
    {
        // getting players rigid body
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        if (Input.GetKey(key))
        {
            // start slide coroutine
            StartCoroutine(Slide());
            // start turn function to turn player 
            Turn();
            
        }
        else if ((!isSliding) && (slidingTimer > 0))
        {
            StopSlide();
        }
    }

    /// <summary>
    /// this function controls the sliding of the player
    /// </summary>
    /// <returns>This returns an amount of time to wait before executing further code</returns>
    private IEnumerator Slide()
    {
        // Set sliding to true 
        isSliding = true;

        // turn off movement script 
        //movement.enabled = false;

        // Set sliding timer to 1
        slidingTimer = 1;
        // apply force to the player to simulate sliding if players velocity is less than max sliding speed
        if (rb.velocity.magnitude < maxSlidingSpeed)
        {
            // add force to the player in the direction of the player
            rb.AddForce(transform.forward * slidingSpeed, ForceMode.Acceleration);
        }
        // stop the players animation temporarily
        GetComponent<Animator>().enabled = false;
        
        // allow the player to move to the left and right while sliding 
        
        // wait until player stops holding down key
        yield return new WaitWhile(() => Input.GetKey(key));
        // start the players animation back up
        GetComponent<Animator>().enabled = true;
        // set sliding to false
        isSliding = false;
    }
    /// <summary>
    /// This function stops the player from sliding
    /// </summary>
    private void StopSlide()
    {
        // set sliding timer to 0
        slidingTimer = 0;
    }

    /// <summary>
    /// This function controls the turning of player through rotation
    /// </summary>
    private void Turn()
    {
        // turning amount per frame
        float turn = Input.GetAxis("Horizontal") * 100f * Time.deltaTime;

        // Quaternion to rotate player 
        Quaternion quaternion = Quaternion.Euler(0, turn, 0);

        // rotate player 
        rb.MoveRotation(rb.rotation * quaternion);
    }
}
