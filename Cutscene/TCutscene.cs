using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCutscene : Trigger
{
    public List<CutsceneEvent> CutsceneEvents;
    private int currentTrigger = -1;

    public override void Activate()
    {
        currentTrigger = 0;
        CutsceneEvents[currentTrigger].Parent = this;
        CutsceneEvents[currentTrigger].Activate();
        CutsceneController.Instance.StartCutscene();
    }

    public void FinishEvent()
    {
        if (currentTrigger >= CutsceneEvents.Count - 1)
        {
            CutsceneController.Instance.StopCutscene();
            Destroy(this);
            return;
        }
        CutsceneEvents[++currentTrigger].Parent = this;
        CutsceneEvents[currentTrigger].Activate();
    }
}
