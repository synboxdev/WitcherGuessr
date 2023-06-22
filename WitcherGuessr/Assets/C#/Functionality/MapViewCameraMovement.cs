using System.Collections.Generic;
using UnityEngine;

public class MapViewCameraMovement : MonoBehaviour
{
    private MapMarkerManager MapMarkerManager;

    public bool IsBeingMoved = false;
    private Camera cam;
    private MapViewCamera MapViewCamera;

    [SerializeField]
    private float zoomStep, minCamSize, maxCamSize, camSize, camResizeRatio;

    [SerializeField]
    private SpriteRenderer mapRenderer;

    private float mapMinX, mapMaxX, mapMinY, mapMaxY;
    private Vector3 dragOrigin;

    void Awake()
    {
        MapMarkerManager = FindObjectOfType<MapMarkerManager>();
        cam = GetComponent<Camera>();
        InitializeMapViewCamera();
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

    public void SetMapForViewing(MapSelection mapSelection)
    {
        MapMarkerManager.EligibleForUserMarking = false;

        if (MapViewCamera.MapSelectionToView != null &&
            MapViewCamera.MapSelectionToView.Index != mapSelection.Index)
        {
            cam.transform.position = MapViewCamera.DefaultPosition;
            cam.orthographicSize = MapViewCamera.DefaultOrthographicSize;
        }

        MapViewCamera.MapSelectionToView = mapSelection;
        ConfigureCameraForViewing();
    }

    public void ZoomIn()
    {
        camSize = cam.orthographicSize - zoomStep;

        cam.orthographicSize = Mathf.Clamp(camSize, minCamSize, Mathf.Min(maxCamSize, (mapRenderer.bounds.size.x / 2f) / cam.aspect));
        cam.transform.position = ClampCamera(cam.transform.position);
    }

    public void ZoomOut()
    {
        camSize = cam.orthographicSize + zoomStep;

        cam.orthographicSize = Mathf.Clamp(camSize, minCamSize, Mathf.Min(maxCamSize, (mapRenderer.bounds.size.x / 2f) / cam.aspect));
        cam.transform.position = ClampCamera(cam.transform.position);
    }

    public void HandleCameraMovement(Vector3 endPosition)
    {
        HandleCameraReposition(endPosition);
        HandleCameraResizing();
    }

    private void HandleCameraReposition(Vector3 endPosition)
    {
        LeanTween.value(cam.gameObject, cam.transform.position, endPosition, 1f)
                 .setEaseOutCubic()
                 .setOnUpdate((Vector3 newPosition) => cam.transform.position = new Vector3(newPosition.x, newPosition.y, cam.transform.position.z));
    }

    private void HandleCameraResizing()
    {
        if (camSize < maxCamSize / 2)
        {
            LeanTween.value(cam.gameObject, camSize, maxCamSize * camResizeRatio, 1f)
                .setEaseOutCubic()
                .setOnUpdate((float newCamSize) => cam.orthographicSize = newCamSize);
        }
    }

    private void InitializeMapViewCamera()
    {
        MapViewCamera = new MapViewCamera()
        {
            DefaultPosition = cam.transform.position,
            DefaultOrthographicSize = cam.orthographicSize
        };
    }

    private void ConfigureCameraForViewing()
    {
        var mapGameObjectToView = MapViewCamera.MapSelectionToView.MapGameObject;
        mapGameObjectToView.SetActive(true);
        mapRenderer = mapGameObjectToView.GetComponent<SpriteRenderer>();

        mapMinX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2f;
        mapMaxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2f;
        mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y / 2f;
        mapMaxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y / 2f;

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minCamSize, Mathf.Min(maxCamSize, (mapRenderer.bounds.size.x / 2f) / cam.aspect));
        ConfigureMapMarkerManager(MapViewCamera.MapSelectionToView);
    }

    private void ConfigureMapMarkerManager(MapSelection mapSelection)
    {
        MapMarkerManager.CurrentMapSelection = new KeyValuePair<MapSelection, GameObject>(mapSelection, mapSelection.MapGameObject);
        LeanTween.value(0, 1, .5f).setOnComplete(() => MapMarkerManager.EligibleForUserMarking = true);
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
            if (dragOrigin != cam.ScreenToWorldPoint(Input.mousePosition))
                IsBeingMoved = true;

            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position = ClampCamera(cam.transform.position + difference);
        }

        if (Input.GetMouseButtonUp(0))
            LeanTween.value(0, 1, .25f).setOnComplete(() => IsBeingMoved = false);
    }
}
