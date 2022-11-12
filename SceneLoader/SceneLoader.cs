using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader current;
    public float Speed;
    public Image Image;
    private string target;
    private float count;
    private bool? beforeChange = null;
    private Color white = Color.black;
    private bool waitFrame;
    public static void LoadScene(string name)
    {
        if (current.beforeChange != null)
        {
            Debug.LogError("What are you doing?!");
        }
        //current.transform.parent = FindObjectOfType<Canvas>().transform;
        current.count = 0;
        current.target = name;
        current.Image.gameObject.SetActive(true);
        current.beforeChange = true;
        Time.timeScale = 0;
        UnityEngine.EventSystems.EventSystem.current.gameObject.SetActive(false);
    }
    private void Awake()
    {
        transform.parent = null;
        if (current != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        else
        {
            current = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Update()
    {
        if (beforeChange != null)
        {
            if (waitFrame)
            {
                waitFrame = false;
                return;
            }
            count += Time.unscaledDeltaTime * Speed;
            white.a = (beforeChange ?? true) ? count : (1 - count);
            Image.color = white;
            if (count >= 1)
            {
                if (beforeChange ?? true)
                {
                    SceneManager.LoadScene(target);
                    count -= 1;
                    beforeChange = false;
                    waitFrame = true;
                }
                else
                {
                    Image.gameObject.SetActive(false);
                    Time.timeScale = 1;
                    count = 0;
                    beforeChange = null;
                }
            }
        }
    }
}
