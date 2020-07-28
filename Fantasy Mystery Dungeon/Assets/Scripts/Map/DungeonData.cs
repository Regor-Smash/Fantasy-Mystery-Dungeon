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

    [SerializeField] private string nextDungeon;
    public string NextDungeon
    {
        get
        {
            return nextDungeon;
        }
    }

    [SerializeField] private bool specialFinalFloor;
    public bool SpecialFinalFloor
    {
        get
        {
            return specialFinalFloor;
        }
    }

    [SerializeField] private int specialFloorRooms;
    public int SpecialFloorRooms
    {
        get
        {
            return specialFloorRooms;
        }
    }

    public DungeonData(int _floorHeight, int _floorWidth, int _totalFloors, string _nextDungeon = "Hub",
                       bool _specialFloor = false, int _specialFloorRooms = 1)
    {
        floorHeight = _floorHeight;
        floorWidth  = _floorWidth;
        totalFloors = _totalFloors;

        nextDungeon = _nextDungeon;

        specialFinalFloor = _specialFloor;
        specialFloorRooms = _specialFloorRooms;
    }
}
