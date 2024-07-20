using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item", order = 1)]
public class ItemSO : ScriptableObject
{
    [SerializeField] protected string Id = "new_item";
    [SerializeField] protected string Name = "New Item";
    [SerializeField] protected string Description = "New Item description";
    [SerializeField] protected Sprite Sprite;
    [SerializeField] protected bool IsStackable = true;
    [SerializeField] protected bool IsEquipable = false;
    [SerializeField] protected bool IsPlayerNameable = false;
    [SerializeField] protected string PlayerName = string.Empty;
    [SerializeField] protected int CurrentStack = 0;
    [SerializeField] protected int MaxStack = 99;
}