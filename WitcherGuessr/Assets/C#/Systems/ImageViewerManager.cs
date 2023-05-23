using UnityEngine;

public class ImageViewerManager : MonoBehaviour
{
    public DragObject CurrentDraggedObject => _currentDraggedObject;

    [SerializeField]
    private RectTransform DefaultLayer = null, DragLayer = null;
    private Rect BoundingBox;
    private DragObject _currentDraggedObject = null;

    void Awake()
    {
        SetBoundingBoxRect(DragLayer);
    }

    private void SetBoundingBoxRect(RectTransform rectTransform)
    {
        var corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        var position = corners[0];

        Vector2 size = new Vector2(
            rectTransform.lossyScale.x * rectTransform.rect.size.x,
            rectTransform.lossyScale.y * rectTransform.rect.size.y);

        BoundingBox = new Rect(position, size);
    }

    public void RegisterDraggedObject(DragObject drag)
    {
        _currentDraggedObject = drag;
        drag.transform.SetParent(DragLayer);
    }

    public void UnregisterDraggedObject(DragObject drag)
    {
        drag.transform.SetParent(DefaultLayer);
        _currentDraggedObject = null;
    }

    public bool IsWithinBounds(Vector2 position)
    {
        return BoundingBox.Contains(position);
    }
}
