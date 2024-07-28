using UnityEngine;

public class ElectricBuilding : Building
{
    public bool IsAttached;
    public bool IsPowered;
    public int UsedCapacity;

    private void Update()
    {
        if (IsAttached)
        {
            IsPowered = GeneratorManager.Instance.Generator.IsOn;
        }
        else
        {
            IsPowered = false;
        }
    }

    public override void InteractWithBuilding()
    {
    }

    public virtual void Attach()
    {
        if (!IsAttached)
        {
            if (GeneratorManager.Instance.Attach(this))
            {
                IsAttached = true;
                return;
            }
            else
            {
                Debug.Log("Not enough capacity to attach " + Name);
            }
        }
        else
        {
            Debug.Log(Name + " is already attached to the generator");
        }
    }

    public virtual void Detach()
    {
        if (IsAttached)
        {
            GeneratorManager.Instance.Detach(this);
            IsAttached = false;
        }
        else
        {
            Debug.Log(Name + " is not attached to the generator");
        }
    }
}