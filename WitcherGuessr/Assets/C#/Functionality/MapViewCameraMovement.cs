using UnityEngine;

public class MapViewCameraMovement : MonoBehaviour
{
    private Camera cam;

    [SerializeField]
    private float zoomStep, minCamSize, maxCamSize;

    [SerializeField]
    private SpriteRenderer mapRenderer;
    private float mapMinX, mapMaxX, mapMinY, mapMaxY;
    private Vector3 dragOrigin;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (mapRenderer != null && mapRenderer.gameObject.activeInHierarchy)
        {
            PanCamera();

            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                ZoomIn();
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                ZoomOut();
        }
    }

    public void SetMapForViewing(GameObject mapObject)
    {
        mapRenderer = mapObject.GetComponent<SpriteRenderer>();

        mapMinX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2f;
        mapMaxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2f;
        mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y / 2f;
        mapMaxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y / 2f;

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minCamSize, Mathf.Min(maxCamSize, (mapRenderer.bounds.size.x / 2f) / cam.aspect));
    }

    public void ZoomIn()
    {
        float newSize = cam.orthographicSize - zoomStep;

        cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, Mathf.Min(maxCamSize, (mapRenderer.bounds.size.x / 2f) / cam.aspect));
        cam.transform.position = ClampCamera(cam.transform.position);
    }

    public void ZoomOut()
    {
        float newSize = cam.orthographicSize + zoomStep;

        cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, Mathf.Min(maxCamSize, (mapRenderer.bounds.size.x / 2f) / cam.aspect));
        cam.transform.position = ClampCamera(cam.transform.position);
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;

        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newY, targetPosition.z);
    }

    private void PanCamera()
    {
        if (Input.GetMouseButtonDown(0))
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position = ClampCamera(cam.transform.position + difference);
        }
    }
}
