using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCutscene : ContinuousTrigger
{
    public List<TCutsceneEvent> CutsceneEvents;
    private int currentTrigger = -1;

    private void Reset()
    {
        TrackDone = true;
    }

    public override void Activate()
    {
        done = false;
        currentTrigger = 0;
        CutsceneEvents[currentTrigger].Parent = this;
        CutsceneEvents[currentTrigger].Activate();
        CutsceneController.Instance.StartCutscene();
    }

    public void FinishEvent()
    {
        if (currentTrigger >= CutsceneEvents.Count - 1)
        {
            done = true;
            CutsceneController.Instance.StopCutscene();
            Destroy(this);
            return;
        }
        CutsceneEvents[++currentTrigger].Parent = this;
        CutsceneEvents[currentTrigger].Activate();
    }
}
