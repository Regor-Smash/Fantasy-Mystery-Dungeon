using UnityEngine;

[CreateAssetMenu(fileName = "NewInventoryItem", menuName = "ScriptableObjects/InventoryItem", order = 0)]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
    public int value;
}
