using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneEvent : MonoBehaviour
{
    [HideInInspector]
    public TCutscene Parent;
    private List<Trigger> triggers;
    private bool active = false;

    private void Start()
    {
        triggers = new List<Trigger>(GetComponents<Trigger>());
    }

    public void Activate()
    {
        foreach (var item in triggers)
        {
            item.Activate();
        }
        active = true;
    }

    private void Update()
    {
        if (!active)
        {
            return;
        }
        if (triggers != null)
        {
            foreach (var item in triggers)
            {
                if (item is ContinuousTrigger && !((ContinuousTrigger)item).Done)
                {
                    return;
                }
            }
        }
        active = false;
        foreach (var item in triggers)
        {
            if (item is ContinuousTrigger)
            {
                ((ContinuousTrigger)item).Deactivate();
            }
        }
        Parent.FinishEvent();
    }
}
