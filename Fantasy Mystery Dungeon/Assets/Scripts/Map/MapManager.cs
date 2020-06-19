using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
	public static MapManager mapMang { get; private set; }
	public HashSet<Vector3> floors { get; private set; }
	public HashSet<Vector3> walls { get; private set; }
	public Vector3 startPos { get; private set; }

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
		startPos = roomOrigins[0];

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

		//[C]orridors
		while (roomOrigins.Count > 1)
		{
			Vector3Int cStart = roomOrigins[0];
			Vector3Int cEnd = roomOrigins[1];
			
			if (cStart.x < cEnd.x)
			{
				for (int xC = cStart.x; xC <= cEnd.x; xC++)
				{
					floorGens.Add(new Vector3Int(xC, cStart.y, cStart.z));
				}
			}
			else if (cStart.x > cEnd.x)
			{
				for (int xC = cStart.x; xC >= cEnd.x; xC--)
				{
					floorGens.Add(new Vector3Int(xC, cStart.y, cStart.z));
				}
			}

			if (cStart.y < cEnd.y)
			{
				for (int yC = cStart.y; yC < cEnd.y; yC++)
				{
					floorGens.Add(new Vector3Int(cEnd.x, yC, cEnd.z));
				}
			}
			else if (cStart.y > cEnd.y)
			{
				for (int yC = cStart.y; yC > cEnd.y; yC--)
				{
					floorGens.Add(new Vector3Int(cEnd.x, yC, cEnd.z));
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
		foreach (Vector3 vF in floors)
		{
			GameObject instF = Instantiate(floorPrefab, vF, Quaternion.identity, mapHolder.transform);
			instF.name = "floor";
		}

		walls  = ListConverter(wallGens);
		foreach (Vector3 vW in walls)
		{
			GameObject instW = Instantiate(wallPrefab, vW, Quaternion.identity, mapHolder.transform);
			instW.name = "wall";
		}
		#endregion PlaceTiles
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

	private void OnDestroy()
	{
		if (mapMang == this)
		{
			mapMang = null;
		}
	}
}
