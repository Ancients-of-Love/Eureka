using UnityEngine;

[CreateAssetMenu(fileName = "Resource Node", menuName = "Resource Node")]
public class ResourceNodeSO : ScriptableObject
{
    public string Name = "New Resource Node";
    public string Id;
    public string Description;
    public int Health = 10;
    public ItemAmount[] DropTable;
}