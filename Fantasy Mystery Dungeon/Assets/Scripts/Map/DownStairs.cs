using UnityEngine;

public class DownStairs : MonoBehaviour
{
    #region MoveEvent
    private void OnEnable()
    {
        PlayerMovement.playerMoved += UseDownStairs;
    }

    private void OnDisable()
    {
        PlayerMovement.playerMoved -= UseDownStairs;
    }
    #endregion MoveEvent

    private void UseDownStairs(string na, Vector3 playerPos)
    {
        if (playerPos == transform.position)
        {
            Debug.Log(na + " used the stairs.");
            MapManager.mapMang.NextFloor();
        }
    }
}
