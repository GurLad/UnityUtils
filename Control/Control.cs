using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SavedData;

public static class Control
{
    public enum CB { Interact, Attack, Pause }
    public enum CM { Keyboard, Controller }
    public enum Axis { X, Y }
    public static CM[] players;
    public static float DeadZone = 0.3f;

    public static void SetPlayers(CM player1, CM player2)
    {
        SetPlayer(0, player1);
        SetPlayer(1, player2);
    }

    public static void SetPlayer(int playerID, CM player)
    {
        if (players == null)
        {
            players = new CM[2];
        }
        players[playerID] = player;
    }

    public static bool GetButton(CB button, int playerID)
    {
        return Input.GetKey(GetKeyCode(button.ToString(), playerID));
    }

    public static bool GetButtonUp(CB button, int playerID)
    {
        return Input.GetKeyUp(GetKeyCode(button.ToString(), playerID));
    }

    public static bool GetButtonDown(CB button, int playerID)
    {
        return Input.GetKeyDown(GetKeyCode(button.ToString(), playerID));
    }

    public static float GetAxis(Axis axis, int playerID)
    {
        if (players[playerID] == CM.Controller)
        {
            float input = Input.GetAxis(axis == Axis.X ? "Horizontal" : "Vertical");
            if (Mathf.Abs(input) > DeadZone)
            {
                return input;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return Input.GetKey(GetKeyCode(axis + "+", playerID)) ? 1 : (Input.GetKey(GetKeyCode(axis + "-", playerID)) ? -1 : 0);
        }
    }

    public static void SetButton(CB button, KeyCode value, int playerID)
    {
        Save(button + SaveNameModifier(playerID), (int)value, SaveMode.Global);
    }

    public static void SetAxis(Axis axis, KeyCode positiveValue, KeyCode negativeValue, int playerID)
    {
        SetAxisPositive(axis, positiveValue, playerID);
        SetAxisNegative(axis, negativeValue, playerID);
    }

    public static void SetAxisPositive(Axis axis, KeyCode positiveValue, int playerID)
    {
        Save(axis + "+" + SaveNameModifier(playerID), (int)positiveValue, SaveMode.Global);
    }

    public static void SetAxisNegative(Axis axis, KeyCode negativeValue, int playerID)
    {
        Save(axis + "-" + SaveNameModifier(playerID), (int)negativeValue, SaveMode.Global);
    }

    public static string DisplayButtonName(string keySaveName, int playerID)
    {
        return GetKeyCode(keySaveName, playerID).ToString();
    }

    private static KeyCode GetKeyCode(string keySaveName, int playerID)
    {
        return (KeyCode)Load(keySaveName + SaveNameModifier(playerID), 0, SaveMode.Global);
    }

    private static string SaveNameModifier(int playerID)
    {
        return "ButtonP" + playerID + "C" + (players[playerID] == CM.Controller ? "T" : "F");
    }
}
