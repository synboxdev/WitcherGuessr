using System.Collections.Generic;
using UnityEngine;

public class MapViewCameraController : MonoBehaviour
{
    private Camera cam;
    private SpriteRenderer MapRenderer;
    private MapMarkerManager MapMarkerManager;
    private MapViewCamera MapViewCamera;

    public bool IsBeingMoved = false;
    public float CamResizeRatio;

    private float zoomStep, minCamSize, maxCamSize, camSize;
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
        if (MapRenderer != null && MapRenderer.gameObject.activeInHierarchy)
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
        ConfigureCameraSettings();
    }

    public void ZoomIn()
    {
        camSize = cam.orthographicSize - zoomStep < minCamSize ? minCamSize : cam.orthographicSize - zoomStep;
        cam.orthographicSize = Mathf.Clamp(camSize, minCamSize, Mathf.Min(maxCamSize, (MapRenderer.bounds.size.x / 2f) / cam.aspect));
        cam.transform.position = ClampCamera(cam.transform.position);
    }

    public void ZoomOut()
    {
        camSize = cam.orthographicSize + zoomStep > maxCamSize ? maxCamSize : cam.orthographicSize + zoomStep;
        cam.orthographicSize = Mathf.Clamp(camSize, minCamSize, Mathf.Min(maxCamSize, (MapRenderer.bounds.size.x / 2f) / cam.aspect));
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
                 .setOnUpdate((Vector3 newPosition) => cam.transform.position = ClampCamera(new Vector3(newPosition.x, newPosition.y, cam.transform.position.z)));
    }

    private void HandleCameraResizing()
    {
        CamResizeRatio = camSize < maxCamSize / 2 ? 1.5f : .4f;

        LeanTween.value(cam.gameObject, camSize, camSize * CamResizeRatio, 1f)
                 .setEaseOutCubic()
                 .setOnUpdate((float newCamSize) => cam.orthographicSize = newCamSize);
    }

    private void InitializeMapViewCamera()
    {
        MapViewCamera = new MapViewCamera()
        {
            DefaultPosition = cam.transform.position,
            DefaultOrthographicSize = cam.orthographicSize
        };
    }

    private void ConfigureCameraSettings()
    {
        var mapSizeDeltaAvg = (MapViewCamera.MapSelectionToView.MapGameObject.GetComponent<RectTransform>().sizeDelta.x +
                              MapViewCamera.MapSelectionToView.MapGameObject.GetComponent<RectTransform>().sizeDelta.y) / 2;
        maxCamSize = 0.2f * mapSizeDeltaAvg;
        minCamSize = maxCamSize * 0.2f;
        camSize = (maxCamSize + minCamSize) / 2;
        cam.orthographicSize = Mathf.Clamp(camSize, minCamSize, Mathf.Min(maxCamSize, (MapRenderer.bounds.size.x / 2f) / cam.aspect));
        cam.transform.position = new Vector3(0, 0, cam.transform.position.z);
        zoomStep = (maxCamSize + minCamSize) / (25 - (Settings.GetMapZoomSensitivity * 1.5f));
    }

    private void ConfigureCameraForViewing()
    {
        var mapGameObjectToView = MapViewCamera.MapSelectionToView.MapGameObject;
        mapGameObjectToView.SetActive(true);
        MapRenderer = mapGameObjectToView.GetComponent<SpriteRenderer>();

        mapMinX = MapRenderer.transform.position.x - MapRenderer.bounds.size.x / 2f;
        mapMaxX = MapRenderer.transform.position.x + MapRenderer.bounds.size.x / 2f;
        mapMinY = MapRenderer.transform.position.y - MapRenderer.bounds.size.y / 2f;
        mapMaxY = MapRenderer.transform.position.y + MapRenderer.bounds.size.y / 2f;

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minCamSize, Mathf.Min(maxCamSize, (MapRenderer.bounds.size.x / 2f) / cam.aspect));
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