using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[CreateAssetMenu(fileName = "Light Emitting Item", menuName = "Items/Light Emitting Item", order = 4)]
public class LightEmittingItemSO : ItemSO
{
    [SerializeField] public float LightIntensity;
    [SerializeField] public float LightRange;
    [SerializeField] public Color LightColor;
    [SerializeField] public bool ShadowsEnabled;
    [SerializeField] public float FuelConsumptionRate = 1;
    [SerializeField] public float FuelLevel = 100;
}