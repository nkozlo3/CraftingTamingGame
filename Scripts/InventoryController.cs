using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{


    public GameObject sword;

    public GameObject dartGun;

    private float amntTimesPressedSword;

    private float amntTimesPressedDartGun;


    private void Start()
    {
        amntTimesPressedSword = 1;

        amntTimesPressedDartGun = 1;
    }

    public void EquipUnequipSword()
    {
        amntTimesPressedSword++;

        if (amntTimesPressedSword % 2 == 0)
        {
            sword.SetActive(true);

            dartGun.SetActive(false);
        } else if (amntTimesPressedSword % 2 != 0)
        {
            sword.SetActive(false);
        }
    }


    public void EquipUnequipDartGun()
    {
        amntTimesPressedDartGun++;
        if (amntTimesPressedDartGun % 2 == 0)
        {
            dartGun.SetActive(true);
            sword.SetActive(false);
        }
        else if (amntTimesPressedDartGun % 2 != 0)
        {
            dartGun.SetActive(false);
        }
    }

}
