﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
	public static MapManager mapMang { get; private set; }
	public HashSet<Vector3> floors { get; private set; }
	public HashSet<Vector3> walls { get; private set; }
	public Vector3 startPos { get; protected set; }
	public Vector3 endPos { get; protected set; }

	[SerializeField] private DungeonData data;
	private int latestFloor = 1;

	private GameObject floorPrefab;
	private GameObject emptyPrefab;

	public int FloorHeight { get { return data.FloorHeight; } }
	public int FloorWidth  { get { return data.FloorWidth;  } }
	
	private void Awake ()
	{
        #region Singleton
        if (mapMang == null)
		{
			mapMang = this;
		}
		else if (mapMang != this)
		{
			Debug.LogError("A MapManager already exists.", mapMang);
			Destroy(this);
		}
        #endregion Singleton

        string sceneName = SceneManager.GetActiveScene().name;
		floorPrefab = Resources.Load<GameObject>("Dungeons/Floors/" + sceneName + " Floor Tile");
		emptyPrefab = Resources.Load<GameObject>("Static Empty");
		
		Generate();
	}

    #region mapGeneration
    private void Generate()
	{
		Vector3Int floorPoint = new Vector3Int(data.FloorWidth * 3 * (latestFloor - 1), 0, 0);
		List<Vector3Int> floorGens;
		if (data.SpecialFinalFloor && latestFloor == data.TotalFloors)
		{
			floorGens = GenerateFloors(data.SpecialFloorRooms, floorPoint);
		}
		else
		{
			floorGens = GenerateFloors(Random.Range(4, 8), floorPoint);
		}
		List<Vector3Int> wallGens = new List<Vector3Int>();
		
		#region PlaceTiles
		GameObject mapHolder = Instantiate(emptyPrefab, floorPoint, Quaternion.identity);
		mapHolder.name = "Floor " + latestFloor.ToString();

		floors = ListConverter(floorGens);
		foreach (Vector3 vF in floors)
		{
			GameObject instF = Instantiate(floorPrefab, vF, Quaternion.identity, mapHolder.transform);
			instF.name = "floor";
		}
		#endregion PlaceTiles
	}
	
	protected virtual List<Vector3Int> GenerateFloors(int roomsAmount, Vector3Int floorOrigin)
	{
		List<Vector3Int> result = new List<Vector3Int>();

		List<Vector3Int> roomOrigins = new List<Vector3Int>();
		while (roomOrigins.Count < roomsAmount)
		{
			Vector3Int newRoom = floorOrigin + new Vector3Int(Random.Range(-data.FloorWidth,  data.FloorWidth  + 1),
															  Random.Range(-data.FloorHeight, data.FloorHeight + 1), 0);

			bool isSafe = true;
			float safeDist = 10f;
			float longDist = 40f;
			for (int ri = 0; ri < roomOrigins.Count; ri++)
			{
				isSafe = (Vector3Int.Distance(newRoom, roomOrigins[ri]) > safeDist) &&
						 (Vector3Int.Distance(newRoom, roomOrigins[ri]) < longDist);
				if (!isSafe) { break; }
			}

			if (isSafe) { roomOrigins.Add(newRoom); }
		}
		result = new List<Vector3Int>(roomOrigins);
		startPos = roomOrigins[0];
		endPos = roomOrigins[roomOrigins.Count - 1];
		Instantiate<GameObject>(Resources.Load<GameObject>("Dungeons/StairsDown"), endPos, Quaternion.identity);

		//fill rooms
		foreach (Vector3Int roomO in roomOrigins)
		{
			int radX = Random.Range(1, 5);
			int radY = Random.Range(1, 5);
			for (int x = -radX; x <= radX; x++)
			{
				for (int y = -radY; y <= radY; y++)
				{
					if (x != 0 || y != 0)
					{
						result.Add(roomO + new Vector3Int(x, y, 0));
					}
				}
			}
		}

		//[C]orridors
		while (roomOrigins.Count > 1)
		{
			Vector3Int cStart = roomOrigins[0];
			Vector3Int cEnd = roomOrigins[1];

			if (cStart.x < cEnd.x)
			{
				for (int xC = cStart.x; xC <= cEnd.x; xC++)
				{
					result.Add(new Vector3Int(xC, cStart.y, cStart.z));
				}
			}
			else if (cStart.x > cEnd.x)
			{
				for (int xC = cStart.x; xC >= cEnd.x; xC--)
				{
					result.Add(new Vector3Int(xC, cStart.y, cStart.z));
				}
			}

			if (cStart.y < cEnd.y)
			{
				for (int yC = cStart.y; yC < cEnd.y; yC++)
				{
					result.Add(new Vector3Int(cEnd.x, yC, cEnd.z));
				}
			}
			else if (cStart.y > cEnd.y)
			{
				for (int yC = cStart.y; yC > cEnd.y; yC--)
				{
					result.Add(new Vector3Int(cEnd.x, yC, cEnd.z));
				}
			}

			roomOrigins.RemoveAt(0);
		}

		return result;
	}

	private HashSet<Vector3> ListConverter (List<Vector3Int> vInts)
	{
		HashSet<Vector3> result = new HashSet<Vector3>();
		
		for (int i = 0; i < vInts.Count; i++)
		{
			result.Add(vInts[i]);
		}

		return result;
	}
	#endregion mapGeneration

	public void NextFloor()
	{
		if (latestFloor < data.TotalFloors) //Move to next floor
		{
			latestFloor++;
			Generate();
			PlayerMovement.currentPlayer.transform.position = startPos;
		}
		else //Move to next dungeon
		{
			SceneManager.LoadScene(data.NextDungeon);

			//Reward
			PlayerInventory.ChangeGold(20);
		}
	}

	private void OnDestroy()
	{
		if (mapMang == this)
		{
			mapMang = null;
		}
	}
}
