using System;
using UnityEngine;
using UnityEngine.UI;

public class ImageInitializationManager : MonoBehaviour
{
    public RectTransform DefaultLayer;
    public GameObject ImageObject;

    void Awake()
    {
        FindObjectOfType<LocationManager>().RegisterImageInitializationManager(this);
    }

    public void SetNewImage(Sprite ImageSprite)
    {
        InitializeImages(ImageSprite);
    }

    private void InitializeImages(Sprite ImageSprite)
    {
        var scaleFactor = Math.Round(DefaultLayer.rect.height / ImageSprite.rect.height, 3);

        // First image 
        var firstImageObject = Instantiate(ImageObject, DefaultLayer.transform);
        firstImageObject.GetComponent<Image>().sprite = ImageSprite;
        var firstImageObjectRect = firstImageObject.GetComponent<RectTransform>();

        firstImageObjectRect.sizeDelta =
                new Vector2((float)Math.Round(ImageSprite.rect.width * scaleFactor),
                            (float)Math.Round(ImageSprite.rect.height * scaleFactor));

        // Second image
        var secondImageObject = Instantiate(ImageObject, DefaultLayer.transform);
        secondImageObject.GetComponent<Image>().sprite = ImageSprite;
        var secondImageObjectRect = secondImageObject.GetComponent<RectTransform>();

        secondImageObjectRect.sizeDelta = new Vector2((float)Math.Round(ImageSprite.rect.width * scaleFactor),
                                                      (float)Math.Round(ImageSprite.rect.height * scaleFactor));

        // Post-initialization
        firstImageObject.GetComponent<DragObject>().NeighborRect = secondImageObjectRect;
        secondImageObject.GetComponent<DragObject>().NeighborRect = firstImageObjectRect;

        firstImageObject.SetActive(true);
        secondImageObject.SetActive(true);
    }

    public void ClearLayers()
    {
        if (DefaultLayer.childCount > 0)
            foreach (Transform child in DefaultLayer.transform)
                Destroy(child.gameObject);
    }
}