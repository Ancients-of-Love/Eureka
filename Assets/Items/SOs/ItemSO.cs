using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item", order = 1)]
public class ItemSO : ScriptableObject
{
    [SerializeField] public string Id = "new_item";
    [SerializeField] public string Name = "New Item";
<<<<<<< Updated upstream
    [SerializeField] public string Description = "New Item description";
    [SerializeField] public Sprite Sprite;
    [SerializeField] public bool IsStackable = true;
    [SerializeField] public bool IsEquipable = false;
    [SerializeField] public bool IsPlayerNameable = false;
    [SerializeField] public string PlayerName = string.Empty;
    [SerializeField] public int CurrentStack = 0;
    [SerializeField] public int MaxStack = 99;
=======
    [SerializeField] public bool IsStackable = true;
    [SerializeField] public string PlayerName = string.Empty;
    [SerializeField] public bool IsPlayerNameable = false;
    [SerializeField] public int CurrentStack = 0;
    [SerializeField] public int MaxStack = 99;
    [SerializeField] protected string Description = "New Item description";
    [SerializeField] protected Sprite Sprite;
    [SerializeField] protected bool IsEquipable = false;
>>>>>>> Stashed changes
}