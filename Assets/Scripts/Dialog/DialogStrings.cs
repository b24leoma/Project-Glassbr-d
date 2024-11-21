using UnityEngine;

[System.Serializable]
public class DialogStrings
{
    public string name;
    
    [TextArea(3, 10)]
    public string[] sentences;
}