using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DungeonTravel : MonoBehaviour
{
	public GameObject dungeonButtonPrefab;
	public Transform dungeonList;
	public GameObject travelCanvas;

	[SerializeField] private string[] dungeons = new string[1];
	
	private void Start()
	{
	    for (int i = 0; i < dungeons.Length; i++)
		{
			GameObject dButt = Instantiate<GameObject>(dungeonButtonPrefab, dungeonList);
			string travelName = dungeons[i].ToString();
			dButt.GetComponentInChildren<Text>().text = travelName;
			dButt.GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene(travelName));
		}
	}

    #region MoveEvent
    private void OnEnable()
	{
		PlayerMovement.playerMoved += OpenTravel;
	}

	private void OnDisable()
	{
		PlayerMovement.playerMoved -= OpenTravel;
	}
    #endregion MoveEvent

    private void OpenTravel(string na, Vector3 playerPos)
	{
		if (playerPos == transform.position)
		{
			travelCanvas.SetActive(true);
		}
	}
}
