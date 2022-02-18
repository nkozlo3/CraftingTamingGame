using UnityEngine;

public class InstantiateCrafts : MonoBehaviour
{

    public GameObject prePrefabCrate;

    public GameObject prePrefabMortarPestle;

    GameObject hand;

    private void Start()
    {
        hand = GameObject.FindGameObjectWithTag("FrontOfHand");
    }

    public void OnClickedCrate()
    {
        if (Harvesting.amntOfWood >= 35)
        {
            Instantiate(prePrefabCrate, hand.transform.position, Quaternion.identity, hand.transform.parent);
            Harvesting.amntOfWood -= 35;
        }
    }

    public void OnClickedMortarPestle()
    {
        if (Harvesting.amntOfStone >= 20 && Harvesting.amntOfHide >= 15)
        {
            Instantiate(prePrefabMortarPestle, hand.transform.position, Quaternion.identity, hand.transform.parent);
            Harvesting.amntOfHide -= 15;
            Harvesting.amntOfStone -= 20;
        }
    }

    public void OnClickedDartGun()
    {
        if (Harvesting.amntOfWood >= 20 && Harvesting.amntOfStone >= 5)
        {
            InstantiateCraftsInInventory.dartGunBought = true;
            Harvesting.amntOfStone -= 5;
            Harvesting.amntOfWood -= 20;
        }
    }


    public void OnClickedNarcoCapsule()
    {
        if (Harvesting.amntOfNarco >= 4)
        {
            Harvesting.amntOfNarcoCapsules++;
            Harvesting.amntOfNarco -= 4;
        }
    }

    public void OnClickedNarcoDart()
    {
        if (Harvesting.amntOfNarcoCapsules >= 1)
        {
            Harvesting.amntOfDarts++;
            Harvesting.amntOfNarcoCapsules -= 1;
        }
    }
}
