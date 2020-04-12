using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCutsceneSpeak : ContinuousTrigger
{
    public List<CutsceneSpeakEvent> Events;
    private int nextEvent;
    public override void Activate()
    {
        CutsceneSpeakController.Instance.StartConversation();
        nextEvent = 0;
        NextEvent();
    }
    public bool NextEvent()
    {
        if (nextEvent >= Events.Count)
        {
            return false;
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
            default:
                break;
        }
        return true;
    }
}

[System.Serializable]
public class CutsceneSpeakEvent
{
    public enum EventType { AddSpeaker, Speak, MoveSpeaker, RemoveSpeaker }
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
}
