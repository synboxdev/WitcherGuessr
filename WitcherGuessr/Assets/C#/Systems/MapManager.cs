using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MapManager : MonoBehaviour
{
    private const string _mapParent = "MapParent";
    private GameObject MapParent;

    private AsyncOperationHandle<Sprite> handle;
    private string addressableDownloadStatusText;
    private bool addressableCompleted = false;
    private MapType initializingMapType;

    public List<MapSelection> MapSelections;

    public GameObject MapSelectionDefault;
    public GameObject MapSelectionSelected;
    public GameObject MapSelectionSelectedAllMaps;
    public GameObject MapSelectionSelectedBAW;
    public GameObject MapSelectionSelectedHOS;

    void Awake()
    {
        if (GameObject.FindGameObjectWithTag(_mapParent) is null)
            InitializeMapParent();
    }

    private void Update()
    {
        if (handle.IsValid() && !handle.IsDone)
        {
            var status = handle.GetDownloadStatus().Percent;
            addressableDownloadStatusText = $"Downloading map - {status * 100:0.00}%";
        }
    }

    public string GetMapDownloadPercentage() => addressableDownloadStatusText;

    public bool GetMapDownloadStatus() => addressableCompleted;

    public GameObject GetUIMapSelectionPrefab(MapType map)
    {
        switch (map)
        {
            case MapType.AllMaps:
                return MapSelectionSelectedAllMaps;
            case MapType.VelenNovigrad:
            case MapType.GauntersWorld:
                return MapSelectionSelectedHOS;
            case MapType.Toussaint:
            case MapType.Fablesphere:
                return MapSelectionSelectedBAW;
            case MapType.Default:
            case MapType.WhiteOrchard:
            case MapType.SkelligeIsles:
            case MapType.KaerMorhen:
            case MapType.IsleOfMists:
            default:
                return MapSelectionSelected;
        }
    }

    public void DisableAllMaps()
    {
        for (int i = 0; i < MapParent.transform.childCount; i++)
            MapParent.transform.GetChild(i).gameObject.SetActive(false);
    }

    public void MapMarkedByUser(MapSelection mapSelection)
    {
        MapSelections.ForEach(x => x.IsMarkedByUser = false);
        MapSelections.FirstOrDefault(x => x.Index == mapSelection.Index).IsMarkedByUser = true;
    }

    public async Task<GameObject> GetInitializedMapAsync(MapSelection mapSelection)
    {
        if (!MapSelections.Any(x => x.MapType == mapSelection.MapType && x.AddressableMapLoaded))
            await InitializeMap(mapSelection);

        return GetMapGameObject(mapSelection);
    }

    public GameObject RegisterLoadedMapGameObject(MapType mapType, Sprite loadedMapSprite)
    {
        var mapSelection = MapSelections.FirstOrDefault(x => x.MapType == mapType);
        mapSelection.MapGameObject = Instantiate(mapSelection.MapPrefab, MapParent.transform);
        mapSelection.MapGameObject.SetActive(false);
        mapSelection.AddressableMapLoaded = true;
        mapSelection.MapGameObject.GetComponent<SpriteRenderer>().sprite = loadedMapSprite;

        return mapSelection.MapGameObject;
    }

    private GameObject GetMapGameObject(MapSelection mapSelection) =>
        MapSelections.FirstOrDefault(x => x.MapType == mapSelection.MapType && x.AddressableMapLoaded).MapGameObject;

    private async Task InitializeMap(MapSelection mapToInitialize)
    {
        addressableCompleted = false;
        handle = mapToInitialize.AddressableMapSprite.LoadAssetAsync();
        handle.Completed += OnAddressableMapLoaded;
        initializingMapType = mapToInitialize.MapType;
        await handle.Task;
    }

    private void OnAddressableMapLoaded(AsyncOperationHandle<Sprite> handle)
    {
        addressableCompleted = true;

        if (handle.Status == AsyncOperationStatus.Succeeded)
            RegisterLoadedMapGameObject(initializingMapType, handle.Result);
        else
            Debug.Log("Error has occurred when loading Addressable map from remote directory!");
    }

    private void InitializeMapParent()
    {
        MapParent = new GameObject()
        {
            name = _mapParent,
            tag = _mapParent
        };
        DontDestroyOnLoad(MapParent);
    }
}