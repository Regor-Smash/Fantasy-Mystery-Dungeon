using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MapManager))]
public class EnemyManager : MonoBehaviour
{
	public GameObject enemyPrefab;
	
	private void Start ()
	{
		SpawnFloorEnemies();
	}
	
	public void SpawnFloorEnemies()
	{
		int enemyNum = 7;
		for (int i = 0; i < enemyNum; i++)
		{
			Instantiate<GameObject>(enemyPrefab, MapManager.mapMang.floors.ElementAt(Random.Range(0, MapManager.mapMang.floors.Count)), Quaternion.identity);
		}
	}
}
