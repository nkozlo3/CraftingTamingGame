using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TooltipsManager : MonoBehaviour
{
    public TextMeshProUGUI tipText;

    public RectTransform tipWindow;

    public static Action<string, Vector2> OnMouseOver;

    public static Action OnMouseOff;

    private void OnEnable()
    {
        OnMouseOver += ShowTip;

        OnMouseOff += HideTip;
    }

    private void OnDisable()
    {
        OnMouseOver -= ShowTip;
        OnMouseOff -= HideTip;
    }

    private void Start()
    {
        HideTip();
    }

    private void ShowTip(string tip, Vector2 mousePos)
    {
        tipText.text = tip;

        tipWindow.sizeDelta = new Vector2(tipText.preferredWidth > 200 ? 200 : tipText.preferredWidth, tipText.preferredHeight);

        tipWindow.gameObject.SetActive(true);

        tipWindow.transform.position = new Vector2(mousePos.x, mousePos.y);
    }

    private void HideTip()
    {
        tipText.text = default;

        tipWindow.gameObject.SetActive(false);
    }
}
