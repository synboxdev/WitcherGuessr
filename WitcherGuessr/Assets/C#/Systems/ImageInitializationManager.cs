using System;
using UnityEngine;
using UnityEngine.UI;

public class ImageInitializationManager : MonoBehaviour
{
    public RectTransform DefaultLayer;
    public GameObject ImageObject;

    [SerializeField]
    private Sprite _imageSprite;

    public void SetNewImage(Sprite ImageSprite)
    {
        ClearLayers();
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

        secondImageObjectRect.position = new Vector2(-secondImageObjectRect.sizeDelta.x + Screen.width,
                                                      secondImageObjectRect.position.y);

        // Post-initialization
        firstImageObject.GetComponent<DragObject>().NeighborRect = secondImageObjectRect;
        secondImageObject.GetComponent<DragObject>().NeighborRect = firstImageObjectRect;

        firstImageObject.SetActive(true);
        secondImageObject.SetActive(true);
    }

    private void ClearLayers()
    {
        foreach (Transform child in DefaultLayer.transform)
            Destroy(child.gameObject);
    }
}