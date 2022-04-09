using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCutsceneEvent : ContinuousTrigger
{
    [HideInInspector]
    public TCutscene Parent;
    private List<Trigger> triggers;
    private bool active = false;

    private void Reset()
    {
        TrackDone = true;
    }

    private void Awake()
    {
        triggers = new List<Trigger>(GetComponents<Trigger>());
        triggers.Remove(this);
    }

    public override void Activate()
    {
        foreach (var item in triggers)
        {
            item.Activate();
        }
        active = true;
        done = false;
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
        done = true;
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
