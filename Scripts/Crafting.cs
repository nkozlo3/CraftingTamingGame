using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{

    public GameObject UI;

    private int amntTimesHitY;
    // Start is called before the first frame update
    void Start()
    {
        amntTimesHitY = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            amntTimesHitY++;
            if (amntTimesHitY % 2 == 0)
            {
                UI.SetActive(true);
            } else if (amntTimesHitY % 2 != 0)
            {
                UI.SetActive(false);
            }
        }
    }
}
