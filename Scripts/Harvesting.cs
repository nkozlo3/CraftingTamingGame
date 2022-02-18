using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Harvesting : MonoBehaviour
{

    public static int amntOfWood = 0;

    public static int amntOfStone = 0;

    public static int amntOfHide = 0;

    public static int amntOfNarco = 0;

    public static int amntOfDarts = 0;

    public static int amntOfBerries = 0;

    public static int amntOfNarcoCapsules;

    public TMP_Text textHide;

    public TMP_Text textNarco;

    public TMP_Text textBerries;

    public TMP_Text textWood;

    public TMP_Text textStone;

    public TMP_Text dartsText;

    public TMP_Text capsulesText;



    private void Update()
    {
        textWood.text = amntOfWood.ToString();

        textStone.text = amntOfStone.ToString();

        textHide.text = amntOfHide.ToString();

        textNarco.text = amntOfNarco.ToString();

        textBerries.text = amntOfBerries.ToString();

        dartsText.text = amntOfDarts.ToString();

        capsulesText.text = amntOfNarcoCapsules.ToString();

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Tree"))
        {
            amntOfWood += Random.Range(2, 5);
        }
        if (collider.CompareTag("Stone"))
        {
            amntOfStone += Random.Range(0, 2);
        }
        if (collider.CompareTag("DeadWarthog"))
        {
            amntOfHide += Random.Range(2, 6);
        }

        if (collider.CompareTag("Bush"))
        {
            int whatToHarvest = Random.Range(-1, 2);
            if (whatToHarvest == 0)
            {
                amntOfNarco += Random.Range(1, 2);
            }
            if (whatToHarvest == 1)
            {
                amntOfBerries += Random.Range(1, 2);
            }
        }
    }
}
