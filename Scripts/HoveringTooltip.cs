using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoveringTooltip : MonoBehaviour
{
    public string tipToShow;

    private bool alreadyHovering = false;

    private float timeBeforeTip = 0.5f;

    public GameObject CraftingTableUI;

    private bool craftingTableUIOpen = false;


    private void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if (!alreadyHovering)
        {
            StopCoroutine(StartTimerForTooltip());

            StartCoroutine(StartTimerForTooltip());
        }

        alreadyHovering = true;
    }

    private void OnMouseExit()
    {
        StopCoroutine(StartTimerForTooltip());

        TooltipsManager.OnMouseOver("", Input.mousePosition);
        alreadyHovering = false;
    }

    private void ShowMessage()
    {
        TooltipsManager.OnMouseOver(tipToShow, Input.mousePosition);
    }


    private IEnumerator StartTimerForTooltip()
    {
        yield return new WaitForSeconds(timeBeforeTip);

        ShowMessage();
    }

    private void OnMouseDown()
    {
        if (!craftingTableUIOpen)
        {
            CraftingTableUI.SetActive(true);
            craftingTableUIOpen = true;
        }
        else if (craftingTableUIOpen)
        {
            CraftingTableUI.SetActive(false);
            craftingTableUIOpen = false;
        }
    }
}
