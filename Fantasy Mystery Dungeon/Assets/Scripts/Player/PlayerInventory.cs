using UnityEngine;
using System.Collections.Generic;

public static class PlayerInventory
{
    public static int gold { get; private set; }

    public static void ChangeGold(int amount)
    {
        if (amount > 0)
        {
            Debug.Log(amount.ToString() + " gold added.");
            gold += amount;
        }
        else if (amount < 0 && amount < gold)
        {
            Debug.Log(amount.ToString() + " gold removed.");
            gold += amount;
        }
    }

    private static Dictionary<ItemData, int> inventory = new Dictionary<ItemData, int>();

    public static void AddItem(ItemData item, int amount)
    {
        Debug.Log("Added " + amount + " " + item.itemName);
        if (inventory.ContainsKey(item))
        {
            inventory[item] += amount;
        }
        else
        {
            inventory[item] = amount;
        }
    }

    public static void AddItem(ItemData item)
    {
        AddItem(item, 1);
    }

    public static List<ItemData> GetItems()
    {
        return new List<ItemData>(inventory.Keys);
    }
}
