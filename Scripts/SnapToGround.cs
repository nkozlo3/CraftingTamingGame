using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToGround : MonoBehaviour
{
    [Tooltip("The object we are crafting")]
    public GameObject craft;

    // Raycast from object to ground
    RaycastHit hit = new RaycastHit();

    // amount of times pressed z 
    private int amntTimesPressedZ = 1;

    // Distance from object to ground 
    private float distanceToGround;

    // Update is called once per frame
    void Update()
    {
        //Finding distance from object to ground
        if (Physics.Raycast(transform.position, Vector3.down, out hit)) { distanceToGround = hit.distance; }

        // snap object to ground if close enough and we press Z
        if (hit.distance < 2f && Input.GetKeyDown(KeyCode.Z))
        {
            //Increment amount of times pressed Z
            amntTimesPressedZ++;

            // if pressed once
            if (amntTimesPressedZ % 2 == 0)
                gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - distanceToGround, transform.position.z); //snap object to ground
            else if (amntTimesPressedZ % 2 != 0) // if pressed again
                Destroy(gameObject); // destroy gameObject
        }

        // if snapped to ground and we press V
        if (amntTimesPressedZ % 2 == 0 && Input.GetKeyDown(KeyCode.V))
        {
            //Instantiate physical crafting object
            Instantiate(craft, gameObject.transform.position, Quaternion.identity);
            //Destroy this pre prefab
            Destroy(gameObject);

        }
    }
}
