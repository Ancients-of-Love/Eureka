using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Timer
{
    public Timer(float time)
    {
        MaxTime = time;
        RemainingTime = MaxTime;
    }

    public float MaxTime { get; private set; }
    public float RemainingTime { get; private set; }

    /// <summary>
    /// Decreases the timer by deltaTime value for each tick.
    /// </summary>
    public bool Tick(float deltaTime)
    {
        if (RemainingTime <= 0f)
        {
            return true;
        }
        RemainingTime -= deltaTime;
        return false;
    }

    /// <summary>
    /// Resets the timer to it's original MaxTime value
    /// </summary>
    public void ResetTimer()
    {
        RemainingTime = MaxTime;
    }

    /// <summary>
    /// Returns true while adding and returns false can't add more
    /// </summary>
    public bool AddTime(float time)
    {
        if (RemainingTime < MaxTime - time)
        {
            RemainingTime += time;
            return true;
        }
        return false;
    }
}