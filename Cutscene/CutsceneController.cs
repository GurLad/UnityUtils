using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour
{
    public static CutsceneController Instance;
    public Image Border1;
    public Image Border2;
    public float ShowBordersSpeed;
    private float fullSize;
    private Vector2 currentSize;
    private float count;
    private int direction;

    private void Start()
    {
        Instance = this;
        fullSize = Border1.rectTransform.sizeDelta.y;
        currentSize = Border1.rectTransform.sizeDelta;
        currentSize.y = 0;
        Border1.rectTransform.sizeDelta = currentSize;
        Border2.rectTransform.sizeDelta = currentSize;
    }

    private void Update()
    {
        if (direction != 0)
        {
            count += Time.deltaTime * ShowBordersSpeed;
            float percent = direction < 0 ? 1 - count : count;
            currentSize.y = fullSize * percent;
            if (percent <= 0)
            {
                currentSize.y = 0;
                direction = 0;
                count = 0;
            }
            if (percent >= 1)
            {
                currentSize.y = fullSize;
                direction = 0;
                count = 0;
            }
            Border1.rectTransform.sizeDelta = currentSize;
            Border2.rectTransform.sizeDelta = currentSize;
        }
    }

    public void StartCutscene()
    {
        direction = 1;
        count = 0;
        //PlayerController.EnableDisablePlayerControl(false);
    }

    public void StopCutscene()
    {
        direction = -1;
        count = 0;
        //PlayerController.EnableDisablePlayerControl(true);
    }
}
