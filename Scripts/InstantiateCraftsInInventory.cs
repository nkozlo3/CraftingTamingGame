using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateCraftsInInventory : MonoBehaviour
{
    [Tooltip("If this player has purchased a dart gun with materials from crafting table")]
    public static bool dartGunBought = false;

    [Tooltip("Button to equip / unequip dartgun in inventory")]
    public GameObject dartGunImageButton;

    private void OnEnable()
    {
        if (dartGunBought == true)
        {
            dartGunImageButton.SetActive(true);
        }
    }
}
