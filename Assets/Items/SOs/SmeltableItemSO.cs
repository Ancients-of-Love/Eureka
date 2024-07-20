using UnityEngine;

[CreateAssetMenu(fileName = "Smeltable Item", menuName = "Items/SmeltableItem", order = 3)]
public class SmeltableItemSO : ItemSO
{
    [SerializeField] private float SmeltTime;
    [SerializeField] private ItemSO SmeltedItem;
}