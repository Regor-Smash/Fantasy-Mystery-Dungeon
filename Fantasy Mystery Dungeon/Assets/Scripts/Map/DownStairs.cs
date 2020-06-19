using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownStairs : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerMovement.playerMoved += UseStairsDown;
    }

    private void OnDisable()
    {
        PlayerMovement.playerMoved -= UseStairsDown;
    }

    private void UseStairsDown(string na, Vector3 playerPos)
    {
        if (playerPos == transform.position)
        {
            Debug.Log(na + " used the stairs.");
        }
    }
}
