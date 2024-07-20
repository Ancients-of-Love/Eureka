using UnityEngine;

[CreateAssetMenu(fileName = "Smeltable Item", menuName = "Items/SmeltableItem", order = 3)]
public class SmeltableItemSO : ItemSO
{
    [SerializeField] public float SmeltTime;
    [SerializeField] public ItemSO SmeltedItem;
}