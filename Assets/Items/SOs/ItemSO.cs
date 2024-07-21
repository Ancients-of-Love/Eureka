using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item", order = 1)]
public class ItemSO : ScriptableObject
{
    public string Id = "new_item";
    public string Name = "New Item";
    public string Description = "New Item description";
    public Sprite Sprite;
    public bool IsStackable = true;
    public bool IsEquipable = false;
    public bool IsPlayerNameable = false;
    public string PlayerName = string.Empty;
    public int CurrentStack = 0;
    public int MaxStack = 99;
    public bool IsResearched = false;
}