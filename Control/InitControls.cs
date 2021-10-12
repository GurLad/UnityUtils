using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitControls : MonoBehaviour
{
    private void Awake()
    {
        Control.SetButton(Control.CB.A, KeyCode.X);
        Control.SetButton(Control.CB.B, KeyCode.Z);
        Control.SetButton(Control.CB.Select, KeyCode.C);
        Control.SetButton(Control.CB.Start, KeyCode.Return);
        Control.SetAxis(Control.Axis.X, KeyCode.RightArrow, KeyCode.LeftArrow);
        Control.SetAxis(Control.Axis.Y, KeyCode.UpArrow, KeyCode.DownArrow);
    }
}
