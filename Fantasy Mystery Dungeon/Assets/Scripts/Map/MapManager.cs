using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
	public static MapManager mapMang { get; private set; }
	public Vector3[] floors { get; private set; }
	public Vector3[] walls { get; private set; }

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

		string sceneName = "Dungeon1";
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
		/*for (int f = -5; f <= 5; f++)
		{
			floorGens.Add(new Vector3Int(f, 0, 0));
			if(Random.value > 0.5)
			{
				floorGens.Add(new Vector3Int(f, 1, 0));
			}
		}*/
		Vector3Int room1 = new Vector3Int(Random.Range(1, 8), 0, 0); //Where the room starts from
		int room1H = Random.Range(2, 6);
		int room1W = Random.Range(2, 6);

		Vector3Int room2 = new Vector3Int(Random.Range(-7, 0), 0, 0); //Where the room starts from
		int room2H = Random.Range(2, 6);
		int room2W = Random.Range(2, 6);

		for (int f = room2.x+1; f <= room1.x-1; f++)//corridor
		{
			floorGens.Add(new Vector3Int(f, 0, 0));
		}

		int r1Offset = Random.Range(0, room1H);
		Debug.Log("Room1: " + room1.ToString() + ",  room1H: " + room1H + ",  room1Offset: " + r1Offset);
		for(int r1H = 0; r1H < room1H; r1H++)
		{
			for (int r1W = 0; r1W < room1W; r1W++)
			{
				floorGens.Add(new Vector3Int(room1.x + r1W, r1H - r1Offset, 0));
			}
		}

		int r2Offset = Random.Range(0, room2H);
		Debug.Log("Room2: " + room2.ToString() + ",  room2H: " + room2H + ",  room2Offset: " + r2Offset);
		for (int r2H = 0; r2H < room2H; r2H++)
		{
			for (int r2W = 0; r2W < room2W; r2W++)
			{
				floorGens.Add(new Vector3Int(room2.x - r2W, r2H - r2Offset, 0));
			}
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
			instF.name = "floor";
		}

		walls  = ListConverter(wallGens);
		for (int wi = 0; wi < walls.Length; wi++)
		{
			GameObject instW = Instantiate(wallPrefab, walls[wi], Quaternion.identity, mapHolder.transform);
			instW.name = "wall";
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
