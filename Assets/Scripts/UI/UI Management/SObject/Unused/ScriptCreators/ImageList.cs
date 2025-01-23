
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[System.Serializable]
public class ImageData
{
    public Image image;
    public string imageName;
}

[CreateAssetMenu(fileName = "ImageList", menuName = "UI/Lists/ImageList")]
public class ImageList : ScriptableObject
{
 public List<ImageData> images;
}

