using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubMapManager : MapManager
{
    protected override List<Vector3Int> GenerateFloors(int roomAmount, Vector3Int roomOrigin)
    {
        startPos = Vector3.zero;

        List<Vector3Int> result = new List<Vector3Int>();

        int hubX = 7;
        int hubY = 4;
        for (int x = -hubX; x <= hubX; x++)
        {
            for (int y = -hubY; y <= hubY; y++)
            {
                result.Add(new Vector3Int(x, y, 0));
            }
        }

        return result;
    }
}
