using UnityEngine;

public class LockToOrigin : MonoBehaviour
{
	private void FixedUpdate ()
	{
	    if (transform.position != Vector3.zero)
		{
			transform.position = Vector3.zero;
		}
		if (transform.rotation != Quaternion.identity)
		{
			transform.rotation = Quaternion.identity;
		}
	}
}
