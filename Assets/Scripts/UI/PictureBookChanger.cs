using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureBookChanger : MonoBehaviour
{
    [SerializeField] private List<Texture2D> images;
    private int currentImage;
    private Image img;

    private void Start()
    {
        img = GetComponent<Image>();
        NextImage();
    }

    public void NextImage()
    {
        if (currentImage < images.Count)
        {
            img.sprite = Sprite.Create(images[currentImage],
                new Rect(0, 0, images[currentImage].width, images[currentImage].height), new Vector2(0.5f, 0.5f));
            currentImage++;
        }
        else
        {
            img.enabled = false;
        }

    }
}
