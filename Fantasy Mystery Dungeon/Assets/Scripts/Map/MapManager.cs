using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
	public static MapManager mapMang { get; private set; }
	public Vector3[] floors { get; private set; }
	public Vector3[] walls { get; private set; }

	private const int playableHeight = 30; //Max y distance from origin
	private const int playableWidth  = 30; //Max x distance from origin

	private GameObject floorPrefab;
	private GameObject wallPrefab;
	private GameObject emptyPrefab;
	
	private void Awake ()
	{
		if(mapMang == null)
		{
			mapMang = this;
		}
		else if (mapMang != this)
		{
			Debug.LogError("A MapManager already exists.", mapMang);
			Destroy(this);
		}

		string sceneName = "Dungeon1"; //Temporary
		floorPrefab = Resources.Load<GameObject>("Dungeons/" + sceneName + "/" + sceneName + " Floor Tile");
		wallPrefab  = Resources.Load<GameObject>("Dungeons/" + sceneName + "/" + sceneName + " Wall Tile");
		emptyPrefab = Resources.Load<GameObject>("Static Empty");
		
		Generate();
	}

	private void Generate()
	{
		GameObject mapHolder = Instantiate(emptyPrefab);
		mapHolder.name = "Map";

		List<Vector3Int> floorGens = new List<Vector3Int>();
		//HashSet<Vector3Int> fl;
		List<Vector3Int> wallGens = new List<Vector3Int>();

		#region GenerateFloors
		int roomsAmount = Random.Range(4, 8);
		Debug.Log("Amount=" + roomsAmount.ToString());
		List<Vector3Int> roomOrigins = new List<Vector3Int>();
		while (roomOrigins.Count < roomsAmount)
		{
			Debug.Log("Count=" + roomOrigins.Count.ToString());
			Vector3Int newRoom = new Vector3Int(Random.Range(-playableWidth,  playableWidth  + 1),
				                                Random.Range(-playableHeight, playableHeight + 1), 0);

			bool isSafe = true;
			float safeDist = 10f;
			float longDist = 40f;
			for(int ri = 0; ri < roomOrigins.Count; ri++)
			{
				isSafe = (Vector3Int.Distance(newRoom, roomOrigins[ri]) > safeDist) &&
					     (Vector3Int.Distance(newRoom, roomOrigins[ri]) < longDist);
				if (!isSafe) { break; }
			}

			if (isSafe) { roomOrigins.Add(newRoom); }
		}
		floorGens = new List<Vector3Int>(roomOrigins);

		//fill rooms
		foreach (Vector3Int roomO in roomOrigins)
		{
			int radX = Random.Range(2, 6);
			int radY = Random.Range(2, 6);
			for (int x = -radX; x <= radX; x++)
			{
				for (int y = -radY; y <= radY; y++)
				{
					if (x != 0 || y != 0)
					{
						floorGens.Add(roomO + new Vector3Int(x,y,0));
					}
				}
			}
		}

		//corridors
		while (roomOrigins.Count > 1)
		{
			Vector3Int start = roomOrigins[0];
			Vector3Int end = roomOrigins[1];
			
			if (start.x < end.x)
			{
				for (int xC = start.x; xC <= end.x; xC++)
				{
					floorGens.Add(new Vector3Int(xC, start.y, start.z));
				}
			}
			else if (start.x > end.x)
			{
				for (int xC = start.x; xC >= end.x; xC--)
				{
					floorGens.Add(new Vector3Int(xC, start.y, start.z));
				}
			}

			if (start.y < end.y)
			{
				for (int yC = start.y; yC < end.y; yC++)
				{
					floorGens.Add(new Vector3Int(end.x, yC, end.z));
				}
			}
			else if (start.y > end.y)
			{
				for (int yC = start.y; yC > end.y; yC--)
				{
					floorGens.Add(new Vector3Int(end.x, yC, end.z));
				}
			}
			
			roomOrigins.RemoveAt(0);
		}
		#endregion GenerateFloors

		#region GenerateWalls
		Vector3Int wallOffset = new Vector3Int(0, 1, 0);
		for (int w = 0; w < floorGens.Count; w++)
		{
			Vector3Int wallPos = floorGens[w] + wallOffset;
			if (!floorGens.Contains(wallPos))
			{
				wallGens.Add(wallPos);
			}
		}
        #endregion GenerateWalls

        #region PlaceTiles
        floors = ListConverter(floorGens);
		for(int fi = 0; fi < floors.Length; fi++)
		{
			GameObject instF = Instantiate(floorPrefab, floors[fi], Quaternion.identity, mapHolder.transform);
			instF.name = "floor"+fi.ToString();
		}

		walls  = ListConverter(wallGens);
		for (int wi = 0; wi < walls.Length; wi++)
		{
			GameObject instW = Instantiate(wallPrefab, walls[wi], Quaternion.identity, mapHolder.transform);
			instW.name = "wall"+wi.ToString();
		}
		#endregion PlaceTiles
	}

	private Vector3[] ListConverter (List<Vector3Int> vInts)
	{
		Vector3[] result = new Vector3[vInts.Count];
		Vector3Int[] v = vInts.ToArray();
		
		for (int i = 0; i < result.Length; i++)
		{
			result[i] = v[i];
		}

		return result;
	}

	private void OnDestroy()
	{
		if (mapMang == this)
		{
			mapMang = null;
		}
	}
}
