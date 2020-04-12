using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContinuousTrigger : Trigger
{
    public bool TrackDone;
    public bool Done
    {
        get
        {
            return !TrackDone || done;
        }
        set
        {
            done = value;
        }
    }
    protected bool done;
    public virtual void Deactivate() { }
}
