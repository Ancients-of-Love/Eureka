using UnityEngine;

[CreateAssetMenu(fileName = "Burnable", menuName = "Items/Burnable Item", order = 2)]
public class BurnableItemSO : ItemSO
{
    [SerializeField] private float BurnTime;
}