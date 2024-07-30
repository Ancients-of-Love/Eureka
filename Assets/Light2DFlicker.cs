using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Light2DFlicker : MonoBehaviour
{
    private Light2D Light2D;

    [Min(0.1f)]
    public float MinIntensity = 0f;
    public float MaxIntensity = 1f;
    public int Smooth = 5;

    private Queue<float> SmoothQueue;
    private float LastSum = 0f;

    public void Reset()
    {
        SmoothQueue.Clear();
        LastSum = 0;
    }

    private void Awake()
    {
        Light2D = GetComponent<Light2D>();

        SmoothQueue = new Queue<float>(Smooth);
    }

    private void Update()
    {
        while (SmoothQueue.Count >= Smooth)
        {
            LastSum -= SmoothQueue.Dequeue();
        }
        float newVal = Random.Range(MinIntensity, MaxIntensity);
        SmoothQueue.Enqueue(newVal);
        LastSum += newVal;

        Light2D.intensity = LastSum / SmoothQueue.Count;
    }
}