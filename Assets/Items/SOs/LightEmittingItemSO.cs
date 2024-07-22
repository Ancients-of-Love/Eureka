using UnityEngine;

[CreateAssetMenu(fileName = "Light Emitting Item", menuName = "Items/Light Emitting Item", order = 4)]
public class LightEmittingItemSO : ItemSO
{
    public float LightIntensity;
    public float LightRange;
    public Color LightColor;
    public bool ShadowsEnabled;
    public float FuelConsumptionRate = 1;
    public float FuelLevel = 100;
}