using UnityEngine;
using UnityEngine.Events;

public class DialogTrigger : MonoBehaviour
{
    public DialogStrings dialogText;
    public UnityEvent onDialogEnd; 

    private DialogManager dialogManager;

    private void Awake()
    {
        dialogManager = FindObjectOfType<DialogManager>();
        
    }

    public void TriggerDialog()
    {
        if (dialogManager != null)
        {
            dialogManager.StartDialog(dialogText, onDialogEnd);
        }
    }
}