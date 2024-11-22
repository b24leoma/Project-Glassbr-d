using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Selection : MonoBehaviour
{
    private int selection;
    [SerializeField] private List<Button> buttons;

    void Start()
    {
        ChangeSelection(-10);
    }
    
    public void ChangeSelection(InputAction.CallbackContext context)
    {
        if (selection == -10) selection = 0;
        else if (context.ReadValue<Vector2>().y > 0.2f)
        {
            selection--;
            if (selection <= 0) selection = 0;
        }
        else if (context.ReadValue<Vector2>().y < -0.2f)
        {
            selection++;
            if (selection >= buttons.Count) selection = buttons.Count - 1;
        }
        ChangeSelection(selection);
    }

    public void OnEnter(InputAction.CallbackContext context)
    {
        buttons[selection].onClick.Invoke();
    }

    public void ChangeSelection(int newSelection)
    {
        selection = newSelection;
        transform.localPosition = new Vector2(0f, selection * -150);
    }
}
