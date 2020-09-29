using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCutsceneSpeak : ContinuousTrigger
{
    public List<CutsceneSpeakEvent> Events;
    private int nextEvent;
    private ContinuousTrigger trigger;
    private void Reset()
    {
        enabled = false;
    }
    private void Update()
    {
        if (trigger != null && trigger.Done)
        {
            trigger = null;
            CutsceneSpeakController.Instance.SpeechUI.SetActive(true);
            NextEvent();
        }
    }
    public override void Activate()
    {
        done = false;
        CutsceneSpeakController.Instance.StartConversation();
        nextEvent = 0;
        NextEvent();
    }
    public void NextEvent()
    {
        enabled = false;
        if (nextEvent >= Events.Count)
        {
            done = true;
            CutsceneSpeakController.Instance.FinishConversation();
            return;
        }
        CutsceneSpeakEvent current = Events[nextEvent++];
        switch (current.Event)
        {
            case CutsceneSpeakEvent.EventType.AddSpeaker:
                CutsceneSpeakController.Instance.AddSpeaker(current.SpeakerName, current.SpeakerIcon, current.FlipX, current.SpeakerPos, current.Pitch);
                NextEvent();
                break;
            case CutsceneSpeakEvent.EventType.Speak:
                CutsceneSpeakController.Instance.Say(current.SpeakerName, current.Text, current.VoiceOver, this);
                break;
            case CutsceneSpeakEvent.EventType.MoveSpeaker:
                CutsceneSpeakController.Instance.MoveSpeaker(current.SpeakerName, current.SpeakerPos, current.FlipX, this);
                break;
            case CutsceneSpeakEvent.EventType.RemoveSpeaker:
                CutsceneSpeakController.Instance.RemoveSpeaker(current.SpeakerName);
                NextEvent();
                break;
            case CutsceneSpeakEvent.EventType.ActivateTrigger:
                current.Trigger.Activate();
                if (current.Trigger is ContinuousTrigger)
                {
                    trigger = (ContinuousTrigger)current.Trigger;
                    enabled = true;
                    CutsceneSpeakController.Instance.SpeechUI.SetActive(false);
                }
                else
                {
                    NextEvent();
                }
                break;
            default:
                break;
        }
        return;
    }
}

[System.Serializable]
public class CutsceneSpeakEvent
{
    public enum EventType { AddSpeaker, Speak, MoveSpeaker, RemoveSpeaker, ActivateTrigger }
    public EventType Event;
    public string SpeakerName;
    //Add
    public Sprite SpeakerIcon;
    public float SpeakerPos = -300; //Move
    public bool FlipX; //Move
    public float Pitch;
    //Speak
    [TextArea(3, 10)]
    public string Text;
    public AudioClip VoiceOver;
    //Activate Trigger
    public Trigger Trigger;
}
