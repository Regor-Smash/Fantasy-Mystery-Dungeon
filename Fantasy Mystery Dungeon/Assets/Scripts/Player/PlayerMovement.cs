using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	private const string horizonInput = "Horizontal";
	private const string vertInput = "Vertical";

	private float moveLerp = 0;
	private float duration = 10;
	private bool isMoving
	{
		get
		{
			return moveIncrement != 0;
		}
	}

	private Vector3 originPos;
	private Vector3 targetPos;
	private float moveIncrement;

	private SpriteRenderer rend;

	public delegate void MoveInfo(string character, Vector3 pos);
	public static event MoveInfo playerMoved;

	public static GameObject currentPlayer { get; private set; }

	private void Start()
	{
		rend = GetComponentInChildren<SpriteRenderer>();
		transform.position = MapManager.mapMang.startPos;
		FollowThis();
	}

	public void FollowThis()
	{
		FollowCam.instance.FollowTarget(transform);
		currentPlayer = gameObject;
	}
	
	private void Update ()
	{
		if (!isMoving) //check for input
		{
			Vector3Int newDirection = Vector3Int.zero;
			if (Input.GetButton(horizonInput)) //X
			{
				int inputDir = Input.GetAxis(horizonInput) > 0 ? 1 : -1;
				//Debug.Log("Move X: " + inputDir.ToString());
				newDirection.x = inputDir;
			}
			if (Input.GetButton(vertInput)) //Y
			{
				int inputDir = Input.GetAxis(vertInput) > 0 ? 1 : -1;
				//Debug.Log("Move Y: " + inputDir.ToString());
				newDirection.y = inputDir;
			}

			if(newDirection != Vector3Int.zero)
			{
				rend.transform.up = newDirection;
				Vector3 newTarget = transform.position + newDirection;
				if (MapManager.mapMang.floors.Contains(newTarget)) //Only move if floor there
				{
					originPos = transform.position;
					targetPos = originPos + newDirection;
					moveIncrement = newDirection.magnitude / duration;
				}
			}
		}
	}

	private void FixedUpdate()
	{
		if (isMoving)
		{
			moveLerp += moveIncrement;
			if (moveLerp < 1)
			{
				transform.position = Vector3.Lerp(originPos, targetPos, moveLerp);
			}
			else //finish movement
			{
				transform.position = targetPos;
				moveLerp = 0;
				moveIncrement = 0;

				if(playerMoved != null) { playerMoved(gameObject.name, targetPos); }
			}
		}
	}

	private void OnDestroy()
	{
		currentPlayer = null;
	}
}
