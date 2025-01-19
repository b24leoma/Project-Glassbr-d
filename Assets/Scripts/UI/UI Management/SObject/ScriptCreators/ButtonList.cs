
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ButtonData
{
    public Button button;
    public string buttonName;
}

[CreateAssetMenu(fileName = "ButtonList", menuName = "UI/Lists/ButtonList")]
public class ButtonList : ScriptableObject
{
    public List<ButtonData> buttons;
}

