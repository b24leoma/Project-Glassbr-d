
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SliderData
{
    public Slider slider;
    public string sliderName;
}

[CreateAssetMenu(fileName = "SliderList", menuName = "UI/Lists/SliderList")]
public class SliderList : ScriptableObject
{
    public List<SliderData> sliders;
}


