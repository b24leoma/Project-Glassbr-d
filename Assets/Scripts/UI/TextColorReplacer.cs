using TMPro;
using UnityEngine;

public class TextColorReplacer : MonoBehaviour
{
    [SerializeField] private Color color1;
    [SerializeField] private string color1ReplacedText = "<1>";
    [SerializeField] private Color color2;
    [SerializeField] private string color2ReplacedText = "<2>";
    public void Start()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.text = text.text.Replace(color1ReplacedText, $"<color=#{ColorUtility.ToHtmlStringRGB(color1)}>");
        text.text = text.text.Replace(color2ReplacedText, $"<color=#{ColorUtility.ToHtmlStringRGB(color2)}>");
    }
}
