using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance = null;
    public static MapSelection mapSelection;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public static void SetMapSelection(MapSelection map)
    {
        mapSelection = map;
    }

    public static MapSelection GetMapSelection()
    {
        return mapSelection;
    }
}