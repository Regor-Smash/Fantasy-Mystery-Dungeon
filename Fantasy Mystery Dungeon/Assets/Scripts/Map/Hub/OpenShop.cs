using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShop : MonoBehaviour
{
    public GameObject shopCanvas;

    private void OnEnable()
    {
        PlayerMovement.playerMoved += OpenShopMenu;
    }

    private void OnDisable()
    {
        PlayerMovement.playerMoved -= OpenShopMenu;
    }

    private void OpenShopMenu(string na, Vector3 playerPos)
    {
        if (playerPos == transform.position)
        {
            Debug.Log(na + " used the shop.");
            shopCanvas.SetActive(true);
        }
    }
}
