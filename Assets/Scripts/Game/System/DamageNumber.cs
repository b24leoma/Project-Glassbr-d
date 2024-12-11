using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float fadeSpeed;
    [SerializeField] private TextMeshPro text;
    private Color alpha;

    public void SetDamage(float damage)
    {
        text.text = damage.ToString();
    }
    void Start()
    {
        //text = GetComponent<TMP_Text>();
        alpha = text.color;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveSpeed * Time.deltaTime * Vector3.up;
        alpha.a -= Time.deltaTime * fadeSpeed;
        text.color = alpha;
        if (alpha.a <= 0) Destroy(gameObject);
    }
}
