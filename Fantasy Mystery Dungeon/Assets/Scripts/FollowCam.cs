using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public static FollowCam instance;
    private Transform target;
    private Vector3 camOffset = new Vector3(0, 0, -10);
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("There is already an instance of FollowCam.", instance);
        }
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = target.position + camOffset;
        }
    }

    public void FollowTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;
        }
    }
}
