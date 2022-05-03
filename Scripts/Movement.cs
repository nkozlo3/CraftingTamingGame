using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{

    private Animator anim;

    private Rigidbody rb;

    private float rotateHorizontal;

    private float rotationSpeed = 100f;

    private float speed = 10f;

    private float moveVertical;

    public GameObject sword;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        rotateHorizontal = Input.GetAxis("Horizontal");

        moveVertical = Input.GetAxis("Vertical");

        anim.SetFloat("Speed", moveVertical);



        if (sword.activeSelf && Input.GetKeyDown(KeyCode.C))
        {
            // Attack animation controller function
            anim.SetTrigger("Attack");
            StartCoroutine(SwordCollider());
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector3(10, 25, 0), ForceMode.Impulse);
        }

    }
    private void FixedUpdate()
    {
        Turn();
        Move();
    }

    private IEnumerator SwordCollider()
    {
        sword.GetComponent<BoxCollider>().enabled = true;

        yield return new WaitForSeconds(1.2f);

        sword.GetComponent<BoxCollider>().enabled = false;
    }

    void Turn()
    {
        // Determine the number of degrees to be turned based on the input, speed and time between frames.
        float turn = rotateHorizontal * rotationSpeed * Time.deltaTime;
        // Make this into a rotation in the y axis.
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        // Apply this rotation to the rigidbody's rotation.
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    /// <summary>
    /// Moves the character in the direction of the vertical vector
    /// </summary>
    private void Move()
    {
        // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
        Vector3 movement = transform.forward * moveVertical * speed * Time.deltaTime;

        // Apply this movement to the rigidbody's position.
        rb.MovePosition(rb.position + movement);
    }
}
