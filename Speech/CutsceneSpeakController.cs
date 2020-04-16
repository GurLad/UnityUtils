using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SoundController;

public class CutsceneSpeakController : MonoBehaviour
{
    public enum CurrentState { Writing, Waiting, Moving }
    public static CutsceneSpeakController Instance;
    public float WriteSpeed;
    public float MoveSpeed;
    [Range(0, 1)]
    public float ListenerFadeStrength;
    public string ButtonName = "Fire1";
    public GameObject SpeechUI;
    public Text Panel;
    public SpeakerIcon BaseSpeaker;
    public AudioClip TypeSound;
    [HideInInspector]
    public CurrentState State;
    private string toWrite;
    private int nextLetter;
    private TCutsceneSpeak origin;
    private float count;
    private SpeakerIcon currentSpeaker;
    private Dictionary<string, float> pitch = new Dictionary<string, float>();
    private List<SpeakerIcon> Speakers = new List<SpeakerIcon>();
    private float targetPos;

    public void StartConversation()
    {
        SpeechUI.SetActive(true);
        //PlayerController.EnableDisablePlayerControl(false);
    }

    public void AddSpeaker(string speaker, Sprite icon, bool flipX, float pos, float pitch)
    {
        if (!this.pitch.ContainsKey(speaker))
        {
            this.pitch.Add(speaker, pitch);
        }
        SpeakerIcon speakerIcon = Instantiate(BaseSpeaker.gameObject, BaseSpeaker.transform.parent).GetComponent<SpeakerIcon>();
        speakerIcon.Name = speaker;
        speakerIcon.Icon.sprite = icon;
        speakerIcon.Icon.rectTransform.localEulerAngles = new Vector3(0, flipX ? 180 : 0, 0);
        speakerIcon.Icon.color = Color.Lerp(Color.black, Color.white, ListenerFadeStrength);
        speakerIcon.NameDisplay.text = speaker;
        speakerIcon.RectTransform.anchoredPosition += new Vector2(pos, 0);
        speakerIcon.gameObject.SetActive(true);
        Speakers.Add(speakerIcon);
        currentSpeaker = speakerIcon;
    }

    public void Say(string speaker, string text, AudioClip voiceOver, TCutsceneSpeak source)
    {
        currentSpeaker.Icon.color = Color.Lerp(Color.black, Color.white, ListenerFadeStrength);
        origin = source;
        PlaySound(voiceOver, true);
        //speakingToPlayer = PlayerController.InteracterID;
        toWrite = text;
        nextLetter = 0;
        Panel.text = "" + toWrite[nextLetter++];
        currentSpeaker = Speakers.Find(a => a.Name == speaker);
        currentSpeaker.Icon.color = Color.white;
        count = 0;
        State = CurrentState.Writing;
    }

    public void MoveSpeaker(string speaker, float pos, bool flipX, TCutsceneSpeak source)
    {
        origin = source;
        currentSpeaker.Icon.color = Color.Lerp(Color.black, Color.white, ListenerFadeStrength);
        Panel.text = "";
        currentSpeaker = Speakers.Find(a => a.Name == speaker);
        currentSpeaker.Icon.rectTransform.localEulerAngles = new Vector3(0, flipX ? 180 : 0, 0);
        targetPos = pos;
        State = CurrentState.Moving;
    }

    public void RemoveSpeaker(string speaker)
    {
        if (pitch.ContainsKey(speaker))
        {
            pitch.Remove(speaker);
        }
        SpeakerIcon speakerIcon = Speakers.Find(a => a.Name == speaker);
        Speakers.Remove(speakerIcon);
        Destroy(speakerIcon.gameObject);
    }

    private void Start()
    {
        Instance = this;
        SpeechUI.SetActive(false);
    }

    private void Update()
    {
        if (origin != null)
        {
            switch (State)
            {
                case CurrentState.Writing:
                    if (Input.GetButtonDown(ButtonName) && nextLetter > 1)
                    {
                        Panel.text = toWrite;
                        PlaySound(TypeSound, pitch[currentSpeaker.Name]);
                        State = CurrentState.Waiting;
                        break;
                    }
                    count += Time.deltaTime * WriteSpeed;
                    if (count >= 1)
                    {
                        count -= 1;
                        Write();
                    }
                    break;
                case CurrentState.Waiting:
                    if (Input.GetButtonDown(ButtonName))
                    {
                        origin.NextEvent();
                    }
                    break;
                case CurrentState.Moving:
                    float speed = MoveSpeed * Mathf.Sign(targetPos - currentSpeaker.RectTransform.anchoredPosition.x) * Time.deltaTime * 60;
                    currentSpeaker.RectTransform.anchoredPosition += new Vector2(speed, 0);
                    if (Mathf.Sign(targetPos - currentSpeaker.RectTransform.anchoredPosition.x) != Mathf.Sign(speed))
                    {
                        currentSpeaker.RectTransform.anchoredPosition = new Vector2(targetPos, currentSpeaker.RectTransform.anchoredPosition.y);
                        origin.NextEvent();
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void FinishConversation()
    {
        foreach (SpeakerIcon speaker in Speakers)
        {
            pitch.Remove(speaker.Name);
            Destroy(speaker.gameObject);
        }
        Speakers.Clear();
        SpeechUI.SetActive(false);
        //PlayerController.EnableDisablePlayerControl(true);
    }

    private void Write()
    {
        Panel.text += toWrite[nextLetter++];
        if (toWrite.ToLower()[nextLetter - 1] >= 'a' && toWrite.ToLower()[nextLetter - 1] <= 'z')
        {
            PlaySound(TypeSound, pitch[currentSpeaker.Name] + (toWrite.ToLower()[nextLetter - 1] - 'm') * 0.01f);
        }
        else if (toWrite.ToLower()[nextLetter - 1] != ' ' && toWrite.ToLower()[nextLetter - 1] != '\n')
        {
            count -= WriteSpeed / 5;
        }
        if (nextLetter >= toWrite.Length)
        {
            State = CurrentState.Waiting;
        }
    }
}
