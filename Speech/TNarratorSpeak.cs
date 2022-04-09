using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//public enum SpeakType { Narrator, Idle, Dialouge }
public class TNarratorSpeak : TSpeak
{
    public float Lifespan = -1;

    private void Start()
    {
        if (VoiceOver != null)
        {
            Lifespan = VoiceOver.length;
        }
    }

    public override void Activate()
    {
        done = false;
        NarratorSpeakController.Instance.Say(Text, VoiceOver, Lifespan, this);
    }
}
