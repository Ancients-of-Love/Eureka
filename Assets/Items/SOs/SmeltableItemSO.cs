using UnityEngine;

[CreateAssetMenu(fileName = "Smeltable Item", menuName = "Items/SmeltableItem", order = 3)]
public class SmeltableItemSO : ItemSO
{
    public float SmeltTime;
    public ItemSO SmeltedItem;
}