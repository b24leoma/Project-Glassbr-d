using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityButton : MonoBehaviour
{

    void Start()
    {
        string input = gameObject.name;
        string numberPart = input.Substring(7);

        int number = int.Parse(numberPart);
        Debug.Log(number);
    }

}

   
