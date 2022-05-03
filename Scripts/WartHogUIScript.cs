using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WartHogUIScript : MonoBehaviour
{
    WartHogScript script;
    // Start is called before the first frame update
    void Start()
    {
        //script = gameObject.GetComponent<WartHogScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// We will create tame mechanics in this function
    /// </summary>
    public void Tame()
    {
        Debug.Log("Entered Tame Function");
        script = GetComponentInParent<WartHogScript>();
        script.sleeping = true;
        // if warthog is sleeping and not fully tamed we can feed it 
        if (script.sleeping /*&& Harvesting.amntOfBerries >= 1*/ && script.amntTamed < 100)
        {
            Debug.Log("Entered If Statement Tamed Class");
            Harvesting.amntOfBerries -= 1;
            script.TamingPercentIncrement(10);
            if (script.amntTamed > 99)
            {
                script.amntTamed = 100;
                script.tamed = true;
            }
            script.tamingPercent.text = script.amntTamed.ToString();
        }
    }
}
