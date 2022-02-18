using System.Collections;
using UnityEngine;

public class TamedScript : MonoBehaviour
{
    public string tipToShow;

    private bool alreadyHovering = false;

    private float timeBeforeTip = 0.5f;

    public GameObject tamedUI;

    private bool tamedUIOpen = false;

    private bool tamed = false;



    private void Update()
    {
        
    }
    private void OnMouseOver()
    {
        if (!tamed) { return; }
        if (!alreadyHovering)
        {
            StopCoroutine(StartTimerForTooltip());

            StartCoroutine(StartTimerForTooltip());
        }

        alreadyHovering = true;
    }

    private void OnMouseExit()
    {
        if (!tamed) { return; }
        StopCoroutine(StartTimerForTooltip());

        TooltipsManager.OnMouseOver("", Input.mousePosition);
        alreadyHovering = false;
    }

    private void ShowMessage()
    {
        if (!tamed) { return; }
        TooltipsManager.OnMouseOver(tipToShow, Input.mousePosition);
    }


    private IEnumerator StartTimerForTooltip()
    {
        yield return new WaitForSeconds(timeBeforeTip);

        ShowMessage();
    }

    private void OnMouseDown()
    {
        if (!tamed) { return; }
        if (!tamedUIOpen)
        {
            tamedUI.SetActive(true);
            tamedUIOpen = true;
        }
        else if (tamedUIOpen)
        {
            tamedUI.SetActive(false);
            tamedUIOpen = false;
        }
    }

    public void Tamed(bool tame)
    {
        tamed = tame;
    }
}
