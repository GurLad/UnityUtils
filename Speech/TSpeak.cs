using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TSpeak : ContinuousTrigger
{
    [TextArea]
    public string Text;
    public AudioClip VoiceOver;
}
