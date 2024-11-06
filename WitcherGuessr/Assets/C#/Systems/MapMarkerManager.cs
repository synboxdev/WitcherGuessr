using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapMarkerManager : MonoBehaviour
{
    private MapViewCameraController MapViewCameraMovement;
    private LineRenderer LineRenderer;
    private MapManager MapManager;
    private LocationManager LocationManager;

    public Camera MapViewCamera;
    public ConfirmGuessButton ConfirmGuessButton;

    [Header("Map and Markers")]
    public KeyValuePair<MapSelection, GameObject>? CurrentMapSelection;
    public bool EligibleForUserMarking = false;
    private bool CorrectMapWasSelected = false;
    private GameObject CorrectMapGameObject = null;

    [Header("User marker")]
    public GameObject UserMarkerPrefab;
    private GameObject UserMarker;
    private bool UserMarkerPlaced = false;

    [Header("Location marker")]
    public GameObject LocationMarkerPrefab;
    private GameObject LocationMarker;
    private bool LocationMarkerPlaced = false;

    [Header("Location area")]
    public GameObject LocationAreaPrefab;
    private GameObject LocationArea;
    private bool LocationAreaPlaced = false;

    [Header("Colors")]
    public Color Yellow;
    public Color Green;
    public Color Red;
    private Color LocationDisplayColor;

    void Awake()
    {
        InitializeInternalSystems();
    }

    void Update()
    {
        if (MapViewCamera.gameObject.activeInHierarchy &&
            !MapViewCameraMovement.IsBeingMoved &&
            EligibleForUserMarking &&
            CurrentMapSelection != null && CurrentMapSelection.Value.Value.gameObject.activeInHierarchy &&
            Input.GetMouseButtonUp(0))
        {
            HandleUserMarker(MapViewCamera.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    public KeyValuePair<bool, float> GetUserMarkerResults()
    {
        var _UserMarkerDistanceToLocationAreaEdge = UserMarkerDistanceToLocationAreaCenter();

        // If User landed Marker within Specific Area's circle, or within inner half of the circle - we consider the accuracy to be 100%.
        if ((IsCorrectLocationInSpecificArea() && _UserMarkerDistanceToLocationAreaEdge <= 100) || _UserMarkerDistanceToLocationAreaEdge < 50)
            _UserMarkerDistanceToLocationAreaEdge = 100;

        return new KeyValuePair<bool, float>(CorrectMapWasSelected, _UserMarkerDistanceToLocationAreaEdge);
    }

    public bool IsUserMarkerPlaced()
    {
        return UserMarkerPlaced;
    }

    public void ResetAllMapMarkers()
    {
        CorrectMapWasSelected = false;
        CorrectMapGameObject = null;

        UserMarkerPlaced = false;
        Destroy(UserMarker);

        LocationMarkerPlaced = false;
        Destroy(LocationMarker);

        LocationAreaPlaced = false;
        Destroy(LocationArea);

        LineRenderer.positionCount = 0;
    }

    public void HandleCorrectLocationDisplay(GameObject correctMapGameObject, bool correctMapWasSelected)
    {
        CorrectMapWasSelected = correctMapWasSelected;
        CorrectMapGameObject = correctMapGameObject;

        HandleLocationMarker();
        HandleLocationArea();

        if (correctMapWasSelected)
            ConnectMarkers();

        DetermineLocationDisplayColor(correctMapWasSelected);
        HandleLocationDisplayColors();
        MapViewCameraMovement.HandleCameraMovement(GetPositionForMainCamera());
    }

    public void DestroyUserMarker()
    {
        Destroy(UserMarker);
    }

    private void HandleLocationMarker()
    {
        if (!LocationMarkerPlaced)
        {
            var locationPosition = LocationManager.GetCurrentLocation().Value.Value.Coordinates;
            LocationMarker = Instantiate(LocationMarkerPrefab, CorrectMapGameObject.transform);
            LocationMarker.transform.position = new Vector3(locationPosition.x, locationPosition.y, -1f);
            LocationMarkerPlaced = true;
        }
        else
        {
            Destroy(LocationMarker);
            LocationMarkerPlaced = false;
        }
    }

    private void HandleLocationArea()
    {
        if (!LocationAreaPlaced)
        {
            LocationArea = Instantiate(LocationAreaPrefab, CorrectMapGameObject.transform);
            var mapSize = CorrectMapGameObject.GetComponent<RectTransform>().sizeDelta;
            var areaScale = ((mapSize.x + mapSize.y) / 2 * 0.025f / LocationArea.GetComponent<CircleCollider2D>().radius) /
                            (IsCorrectLocationInSpecificArea() ? 4 : 1);
            LocationArea.GetComponent<Transform>().localScale = new Vector3(areaScale, areaScale, areaScale);
            LocationArea.transform.position = new Vector3(LocationMarker.transform.localPosition.x, LocationMarker.transform.localPosition.y, -1f);
            LocationAreaPlaced = true;
        }
        else
        {
            Destroy(LocationArea);
            LocationAreaPlaced = false;
        }
    }

    private bool IsCorrectLocationInSpecificArea()
    {
        var specificAreas = CorrectMapGameObject.transform.GetComponentsInChildren<PolygonCollider2D>();
        return specificAreas.Any() && specificAreas.Any(specificArea => specificArea.OverlapPoint(LocationMarker.transform.localPosition));
    }

    private void ConnectMarkers()
    {
        LineRenderer.positionCount = 2;
        LineRenderer.SetPosition(0, new Vector3(UserMarker.transform.localPosition.x,
                                                UserMarker.transform.localPosition.y,
                                                0));

        LineRenderer.SetPosition(1, new Vector3(LocationMarker.transform.localPosition.x,
                                                LocationMarker.transform.localPosition.y,
                                                0));
    }

    private void HandleLocationDisplayColors()
    {
        LocationArea.GetComponent<SpriteRenderer>().color = LocationDisplayColor;
        LineRenderer.startColor = LocationDisplayColor;
        LineRenderer.endColor = LocationDisplayColor;
    }

    private Vector3 GetPositionForMainCamera()
    {
        return LocationMarker.transform.localPosition;
    }

    private void DetermineLocationDisplayColor(bool correctMapWasSelected)
    {
        var _UserMarkerDistanceToLocationAreaEdge = UserMarkerDistanceToLocationAreaCenter();

        if (!correctMapWasSelected || _UserMarkerDistanceToLocationAreaEdge > 100)
            LocationDisplayColor = Red;
        else if ((IsCorrectLocationInSpecificArea() && _UserMarkerDistanceToLocationAreaEdge <= 100) || _UserMarkerDistanceToLocationAreaEdge < 50)
            LocationDisplayColor = Green;
        else if (_UserMarkerDistanceToLocationAreaEdge <= 100 && _UserMarkerDistanceToLocationAreaEdge >= 50)
            LocationDisplayColor = Yellow;
    }

    private float UserMarkerDistanceToLocationAreaCenter()
    {
        var locationAreaCircleCollider = LocationArea.GetComponent<CircleCollider2D>();
        var distanceToCenter = Vector3.Distance(LocationArea.transform.localPosition, UserMarker.transform.localPosition);
        var locationAreaExtents = locationAreaCircleCollider.bounds.extents.x * LocationArea.transform.localScale.x;
        return distanceToCenter * 100 / locationAreaExtents;
    }

    private void HandleUserMarker(Vector3 inputPosition)
    {
        if (!UserMarkerPlaced)
            PlaceUserMarker();

        UserMarker.transform.SetParent(CurrentMapSelection.Value.Value.transform);
        UserMarker.transform.position = new Vector3(inputPosition.x, inputPosition.y, -1f);
        MapManager.MapMarkedByUser(CurrentMapSelection.Value.Key);
    }

    private void PlaceUserMarker()
    {
        UserMarker = Instantiate(UserMarkerPrefab, CurrentMapSelection.Value.Value.transform);
        UserMarkerPlaced = true;
        ConfirmGuessButton.ToggleActive(UserMarkerPlaced);
    }

    private void InitializeInternalSystems()
    {
        LineRenderer = GetComponent<LineRenderer>();
        MapViewCameraMovement = FindFirstObjectByType<MapViewCameraController>();
        MapManager = FindFirstObjectByType<MapManager>();
        LocationManager = FindFirstObjectByType<LocationManager>();
        MapViewCamera.gameObject.SetActive(false);
    }
}