using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchDisplay : MonoBehaviour
{
    [SerializeField] private LightEmittingItemSO Item;
    [SerializeField] private Light2D Light;
    [SerializeField] private bool LightOn = true;
    [SerializeField] private bool LightOnOverride = true;
    [SerializeField] private float FuelConsumptionRate;
    [SerializeField] private float FuelLevel;

    // Start is called before the first frame update
    private void Start()
    {
        Light.intensity = Item.LightIntensity;
        Light.color = Item.LightColor;
        Light.pointLightInnerRadius = Item.LightRange;
        Light.pointLightOuterRadius = Item.LightRange * 5f;
        Light.shadowsEnabled = Item.ShadowsEnabled;
        FuelConsumptionRate = Item.FuelConsumptionRate;
        FuelLevel = Item.FuelLevel;
    }

    private void FixedUpdate()
    {
        if (FuelLevel <= 0 || !LightOnOverride)
        {
            ToggleLight(false);
        }
        else if (FuelLevel > 0 && !LightOn && LightOnOverride)
        {
            ToggleLight(true);
        }
        if (FuelLevel > 0 && LightOn && LightOnOverride)
        {
            FuelLevel -= FuelConsumptionRate;
        }
    }

    private void ToggleLight(bool toggleOn)
    {
        if (LightOn && !toggleOn)
        {
            Light.intensity = 0;
            Light.color = Color.black;
            Light.pointLightInnerRadius = 0;
            Light.pointLightOuterRadius = 0;
            Light.shadowsEnabled = false;
            LightOn = false;
        }
        else if (!LightOn && toggleOn)
        {
            Light.intensity = Item.LightIntensity;
            Light.color = Item.LightColor;
            Light.pointLightInnerRadius = Item.LightRange;
            Light.pointLightOuterRadius = Item.LightRange * 5f;
            Light.shadowsEnabled = Item.ShadowsEnabled;
            LightOn = true;
        }
    }
}