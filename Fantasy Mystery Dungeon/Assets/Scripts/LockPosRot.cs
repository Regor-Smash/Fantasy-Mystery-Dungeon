using UnityEngine;

public class LockPosRot : MonoBehaviour
{
	private Vector3 pos;
	private Quaternion rot;
	
	private void Start ()
	{
		pos = transform.position;
		rot = transform.rotation;
	}
	
	private void FixedUpdate ()
	{
		transform.position = pos;
		transform.rotation = rot;
	}
}
