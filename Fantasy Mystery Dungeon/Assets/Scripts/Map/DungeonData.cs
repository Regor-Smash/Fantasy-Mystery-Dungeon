using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonData
{
    [SerializeField] private int floorHeight;
    public int FloorHeight
    {
        get
        {
            return floorHeight;
        }
    }

    [SerializeField] private int floorWidth;
    public int FloorWidth
    {
        get
        {
            return floorWidth;
        }
    }

    [SerializeField] private int totalFloors;
    public int TotalFloors
    {
        get
        {
            return totalFloors;
        }
    }
}
