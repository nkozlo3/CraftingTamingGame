using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    //Starting Health
    private float startingHealth = 100;

    // Current Health
    public static float currentHealth;

   


    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //Setting currentHealth to startingHealth
        currentHealth = startingHealth;
    }

    
    void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
        
    //    if (collision.collider.tag == "Leopard")
    //    {
    //        Debug.Log("PlayerHitByLeopard");
    //        rb.AddForce(new Vector3(0, 10, 0), ForceMode.Impulse);
    //    }
    //}
}
