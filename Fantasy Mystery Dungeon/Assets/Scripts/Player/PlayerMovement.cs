using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	private const string horizonInput = "Horizontal";
	private const string vertInput = "Vertical";

	private float moveLerp = 0;
	private float duration = 20;
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

	public delegate void MoveInfo(string character, Vector3 pos);
	public static event MoveInfo playerMoved;

	private void Start()
	{
		//rend = GetComponent<SpriteRenderer>();
		transform.position = MapManager.mapMang.startPos;
		LookAt();
	}

	public void LookAt()
	{
		FollowCam.instance.FollowTarget(transform);
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
}
