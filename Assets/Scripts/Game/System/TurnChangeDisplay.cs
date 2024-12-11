using TMPro;
using UnityEngine;

public class TurnChangeDisplay : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveDuration;
    [SerializeField] private float stayDuration;
    [SerializeField] private TextMeshProUGUI text;
    private bool moving;
    private float moveTimer;
    private float stayTimer;
    private Vector3 startPos;

    void OnEnable()
    {
        startPos = transform.position;
    }

    public void Display(string displayText)
    {
        moving = true;
        text.text = displayText;
        transform.position = startPos;
        moveTimer = 0;
        stayTimer = 0;
    }
    
    void Update()
    {
        if (moving)
        {
            if (moveTimer < moveDuration && stayTimer == 0)
            {
                transform.position += Time.deltaTime * moveSpeed * Vector3.up;
                moveTimer += Time.deltaTime;
            }
            else if (stayTimer < stayDuration)
            {
                stayTimer += Time.deltaTime;
            }
            else
            {
                transform.position += Time.deltaTime * moveSpeed * Vector3.down;
                moveTimer -= Time.deltaTime;
                if (moveTimer <= 0)
                {
                    transform.position = startPos;
                    moving = false;
                }
            }
        }
    }
}
