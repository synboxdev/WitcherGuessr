using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class DragObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public bool isBeingDragged = false;
    public float DistanceBetweenDiff;
    public RectTransform NeighborRect;

    private RectTransform thisRect;
    private ImageViewerManager ImageViewerManager = null;

    void LateUpdate()
    {
        DistanceBetweenDiff = thisRect.sizeDelta.x - Vector3.Distance(transform.position, NeighborRect.position);

        if (!isBeingDragged)
        {
            if (transform.position.x < 0)
                transform.position = new Vector2(transform.position.x + DistanceBetweenDiff * -1, transform.position.y);
            else
                transform.position = new Vector2(transform.position.x + DistanceBetweenDiff, transform.position.y);
        }

        if (transform.position.x > thisRect.sizeDelta.x)
            Reposition(Side.Left);
        else if (Mathf.Abs(transform.position.x) > thisRect.sizeDelta.x)
            Reposition(Side.Right);
    }

    void Awake()
    {
        thisRect = transform.GetComponent<RectTransform>();
        ImageViewerManager = FindObjectOfType<ImageViewerManager>();
    }

    void Reposition(Side side)
    {
        if (side == Side.Left)
            thisRect.position = new Vector2(-thisRect.sizeDelta.x + Screen.width, thisRect.position.y);

        if (side == Side.Right)
            transform.position = new Vector2(thisRect.sizeDelta.x, thisRect.position.y);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ImageViewerManager.RegisterDraggedObject(this);
        isBeingDragged = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.Translate(new Vector3(eventData.delta.x, transform.localPosition.y));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ImageViewerManager.UnregisterDraggedObject(this);
        isBeingDragged = false;
    }
}