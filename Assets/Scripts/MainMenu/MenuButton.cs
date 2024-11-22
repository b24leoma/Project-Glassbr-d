using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private Transform menuSelect;
    private TextMeshProUGUI textMesh;
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void MouseEnter(int selection) //https://discussions.unity.com/t/solve-onmouseenter-not-working-on-ui-elements/142738
    {
        menuSelect.GetComponent<Selection>().ChangeSelection(selection);
    }
}
