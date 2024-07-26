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

    [SerializeField] private Timer Timer;

    private bool LightToggle = false;
    private bool ReloadToggle = false;

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
        Timer = new(FuelLevel);
    }

    private void Update()
    {
        HandleLighting();
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

    private void HandleLighting()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            LightToggle = !LightToggle;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadToggle = !ReloadToggle;
        }

        if (ReloadToggle)
        {
            if (!Timer.AddTime(1f * Time.deltaTime * 10f))
            {
                ReloadToggle = false;
            }
        }

        if (LightToggle)
        {
            if (!Timer.Tick(FuelConsumptionRate * Time.deltaTime))
            {
                ToggleLight(true);
            }
            else
            {
                ToggleLight(false);
            }
        }
        else
        {
            ToggleLight(false);
        }
    }
}