using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitControls : MonoBehaviour
{
    private void Awake()
    {
        Control.SetPlayer(0, Control.CM.Keyboard); //TEMP!!!
        if (!SavedData.HasKey("InitControls", SaveMode.Global))
        {
            SavedData.Save("InitControls", 1, SaveMode.Global);
            Control.SetPlayer(0, Control.CM.Keyboard);
            Control.SetButton(Control.CB.Attack, KeyCode.Space, 0);
            Control.SetButton(Control.CB.Interact, KeyCode.E, 0);
            Control.SetButton(Control.CB.Pause, KeyCode.Q, 0);
            Control.SetAxis(Control.Axis.X, KeyCode.D, KeyCode.A, 0);
            Control.SetAxis(Control.Axis.Y, KeyCode.W, KeyCode.S, 0);
            Control.SetPlayer(1, Control.CM.Keyboard);
            Control.SetButton(Control.CB.Attack, KeyCode.Keypad8, 1);
            Control.SetButton(Control.CB.Interact, KeyCode.Keypad4, 1);
            Control.SetButton(Control.CB.Pause, KeyCode.Keypad6, 1);
            Control.SetAxis(Control.Axis.X, KeyCode.RightArrow, KeyCode.LeftArrow, 1);
            Control.SetAxis(Control.Axis.Y, KeyCode.UpArrow, KeyCode.DownArrow, 1);
        }
    }
}
