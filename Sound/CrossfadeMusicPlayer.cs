using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CrossfadeMusicPlayerObject
{
    public string Name;
    public AudioClip AudioClip;
}

public class CrossfadeMusicPlayer : MonoBehaviour
{
    public static CrossfadeMusicPlayer Instance;
    public List<CrossfadeMusicPlayerObject> Tracks;
    public float FadeSpeed;
    [Range(0,1)]
    public float Volume = 1;
    public bool PlayOnStart;
    public bool KeepTimestamp;
    private AudioSource mainAudioSource;
    private AudioSource seconderyAudioSource;
    private float count;
    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
        mainAudioSource = gameObject.AddComponent<AudioSource>();
        seconderyAudioSource = gameObject.AddComponent<AudioSource>();
        mainAudioSource.loop = seconderyAudioSource.loop = true;
        mainAudioSource.volume = Volume;
        seconderyAudioSource.volume = 0;
        if (PlayOnStart)
        {
            mainAudioSource.clip = Tracks[0].AudioClip;
            mainAudioSource.Play();
        }
    }
    public void Play(string name)
    {
        if ((seconderyAudioSource.clip = Tracks.Find(a => a.Name == name).AudioClip) == null)
        {
            throw new System.Exception("No matching audio clip!");
        }
        mainAudioSource.volume = Volume;
        seconderyAudioSource.volume = 0;
        seconderyAudioSource.Play();
        count = 0;
        if (KeepTimestamp && seconderyAudioSource.clip.length >= mainAudioSource.time)
        {
            seconderyAudioSource.time = mainAudioSource.time;
        }
        else
        {
            seconderyAudioSource.time = 0;
        }
    }
    private void Update()
    {
        if (seconderyAudioSource.clip != null)
        {
            count += Time.deltaTime * FadeSpeed;
            if (count >= 1)
            {
                AudioSource temp = mainAudioSource;
                mainAudioSource = seconderyAudioSource;
                seconderyAudioSource = temp;
                mainAudioSource.volume = Volume;
                seconderyAudioSource.volume = 0;
                seconderyAudioSource.clip = null;
                seconderyAudioSource.Stop();
                count = 0;
            }
            else
            {
                mainAudioSource.volume = Volume * (1 - count);
                seconderyAudioSource.volume = Volume * count;
            }
        }
    }
}
