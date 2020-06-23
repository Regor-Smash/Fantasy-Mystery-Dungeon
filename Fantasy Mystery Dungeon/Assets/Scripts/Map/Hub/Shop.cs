using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public ItemData[] saleItems;
    public Transform itemLister;
    public GameObject itemButtonPrefab;

    private const float sellMod = 0.5f;

    public ItemData startItem;

    private void Start()
    {
        BuyMode();

        //Place holder starting inventory
        PlayerInventory.ChangeGold(500);
        PlayerInventory.AddItem(startItem, 5);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) //Place holder until inventory UI
        {
            Debug.Log("Gold: "  + PlayerInventory.gold.ToString());
            Debug.Log("Items: " + PlayerInventory.GetItems().ToString());
        }
    }

    private void ClearButtons()
    {
        for (int i = 0; i < itemLister.childCount; i++)
        {
            Destroy(itemLister.GetChild(0).gameObject);
        }
    }

    public void BuyMode()
    {
        ClearButtons();

        for (int i = 0; i < saleItems.Length; i++)
        {
            ItemData id = saleItems[i];
            GameObject butt = Instantiate<GameObject>(itemButtonPrefab, itemLister);
            butt.GetComponentInChildren<Text>().text = id.itemName;
            butt.GetComponent<Button>().onClick.AddListener( () => Buy(id));
        }
    }

    public void Buy(ItemData item)
    {
        if (PlayerInventory.gold >= item.goldValue)
        {
            PlayerInventory.ChangeGold(-item.goldValue);
            PlayerInventory.AddItem(item);
        }
    }

    public void SellMode()
    {
        ClearButtons();

        for (int i = 0; i < PlayerInventory.GetItems().Count; i++)
        {
            ItemData id = PlayerInventory.GetItems()[i];
            GameObject butt = Instantiate<GameObject>(itemButtonPrefab, itemLister);
            butt.GetComponentInChildren<Text>().text = id.itemName;
            butt.GetComponent<Button>().onClick.AddListener(() => Sell(id));
        }
    }

    public void Sell(ItemData item)
    {
        if (PlayerInventory.GetItems().Contains(item))
        {
            PlayerInventory.AddItem(item, -1);
            PlayerInventory.ChangeGold((int)(item.goldValue * sellMod));
        }
    }
}
