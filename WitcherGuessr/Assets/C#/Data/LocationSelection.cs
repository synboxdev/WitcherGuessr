using System;
using System.Collections.Generic;

[Serializable]
public class LocationSelection
{
    public MapType MapType;
    public string DefaultLocationName;
    public List<Location> LocationsForViewing;
}