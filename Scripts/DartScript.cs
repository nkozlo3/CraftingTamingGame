using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartScript : MonoBehaviour
{
    public GameObject characterUI;

    public GameObject characterUIObjects;

    public static bool zoomed = false;

    private bool alreadyCenterScreen = false;

    private Vector3 centerScreen;

    private void OnEnable()
    {
        characterUI.SetActive(true);

        centerScreen = characterUIObjects.transform.position;
    }

    private void OnDisable()
    {
        characterUI.SetActive(false);
    }

    private void LateUpdate()
    {
        if (zoomed == true)
        {
            alreadyCenterScreen = false;
            characterUIObjects.transform.position = Input.mousePosition;
        }
        else if (alreadyCenterScreen == false && zoomed != true)
        {
            characterUIObjects.transform.position = centerScreen;
            alreadyCenterScreen = true;        
        }

        //RaycastHit hitInfo;
        //if(Physics.Raycast(Camera.main.transform.forward, Camera.main.transform.forward, out hitInfo, 10f, LayerMask.GetMask("Ignore Raycast")))
        //{
        //    GameObject thing = hitInfo.collider.gameObject;
        //}
        Shooting();
    }

    private void Shooting()
    {
        if (Input.GetKeyDown(KeyCode.C) /*&& Harvesting.amntOfDarts > 0*/)
        {
            Harvesting.amntOfDarts--;

            if (zoomed) 
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //RaycastHit hit;
                if (Physics.Raycast(ray, out RaycastHit hit, 100))
                {
                    Debug.Log(hit.transform.tag);
                    if (hit.transform.CompareTag("Warthog"))
                    {
                        //WartHogScript.amntNarcotics++;
                        WartHogScript whScript = hit.transform.GetComponent<WartHogScript>();
                        whScript.IncreaseNarcos(1);
                    }
                }
            }
            if (!zoomed)
            {
                Ray ray = Camera.main.ScreenPointToRay(characterUIObjects.transform.position);
                //RaycastHit hit;
                if (Physics.Raycast(ray, out RaycastHit hit, 100))
                {
                    Debug.Log(hit.transform.tag);
                    if (hit.transform.CompareTag("Warthog"))
                    {
                        Debug.Log("THIS PART WORKS YAY");
                        //WartHogScript.amntNarcotics++;
                        WartHogScript whScript = hit.transform.GetComponent<WartHogScript>();
                        whScript.IncreaseNarcos(1);
                    }
                }
            }
        }
    }
}
