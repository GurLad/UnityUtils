using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SoundController;

public class NarratorSpeakController : MonoBehaviour
{
    public static NarratorSpeakController Instance;
    private enum CurrentMode { Show, Wait, Hide, Delay, Sleep }
    public Text Text;
    public Image TextBG;
    public float TransitionTime;
    public float PostSpeakDelay;
    public float BGAlpha;
    [HideInInspector]
    public float Lifespan;
    private float count;
    private CurrentMode currentMode = CurrentMode.Sleep;
    private Color textColor;
    private Color textBGColor;
    private TNarratorSpeak origin;

    private void Awake()
    {
        Instance = this;
        textColor = Text.color;
        textBGColor = TextBG.color;
        textColor.a = 0;
        textBGColor.a = 0;
        Text.color = textColor;
        TextBG.color = textBGColor;
    }

    public void Say(string text, AudioClip voiceOver, float lifespan = -1, TNarratorSpeak source = null)
    {
        origin = source;
        if (lifespan != -1)
        {
            Lifespan = lifespan;
        }
        Text.text = text;
        textColor.a = 0;
        textBGColor.a = 0;
        count = 0;
        PlaySound(voiceOver);
        currentMode = CurrentMode.Show;
    }

    private void Update()
    {
        float percent;
        switch (currentMode)
        {
            case CurrentMode.Show:
                count += Time.deltaTime;
                if (count >= TransitionTime)
                {
                    textBGColor.a = BGAlpha;
                    textColor.a = 1;
                    count = 0;
                    currentMode = CurrentMode.Wait;
                    break;
                }
                percent = count / TransitionTime;
                textBGColor.a = BGAlpha * percent;
                textColor.a = 1 * percent;
                break;
            case CurrentMode.Wait:
                count += Time.deltaTime;
                if (count >= Lifespan)
                {
                    count = 0;
                    currentMode = CurrentMode.Hide;
                    break;
                }
                break;
            case CurrentMode.Hide:
                count += Time.deltaTime;
                if (count >= TransitionTime)
                {
                    textBGColor.a = 0;
                    textColor.a = 0;
                    Text.color = textColor;
                    TextBG.color = textBGColor;
                    count = 0;
                    currentMode = CurrentMode.Delay;
                    break;
                }
                percent = 1 - count / TransitionTime;
                textBGColor.a = BGAlpha * percent;
                textColor.a = 1 * percent;
                break;
            case CurrentMode.Delay:
                count += Time.deltaTime;
                if (count >= PostSpeakDelay)
                {
                    currentMode = CurrentMode.Sleep;
                    if (origin != null)
                    {
                        origin.Done = true;
                    }
                }
                break;
            default:
                return;
        }
        Text.color = textColor;
        TextBG.color = textBGColor;
    }
}
