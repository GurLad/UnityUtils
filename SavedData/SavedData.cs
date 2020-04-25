using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SaveMode { Slot, Global }
public static class SavedData
{
    public const int MAX_NUM_SLOTS = 5;
    public static int SaveSlot;
    /// <summary>
    /// Saves a string, int or float.
    /// </summary>
    /// <typeparam name="T">The type to save (string, int or float).</typeparam>
    /// <param name="dataName">The filename.</param>
    /// <param name="data">The data to save.</param>
    public static void Save<T>(string dataName, T data, SaveMode saveMode = SaveMode.Slot)
    {
        dataName = (saveMode != SaveMode.Global ? SaveSlot.ToString() : "") + dataName;
        Type selectedType = typeof(T);
        if (selectedType == typeof(string))
        {
            PlayerPrefs.SetString(dataName, data.ToString());
        }
        else if (selectedType == typeof(int))
        {
            PlayerPrefs.SetInt(dataName, Convert.ToInt32(data));
        }
        else if (selectedType == typeof(float))
        {
            PlayerPrefs.SetFloat(dataName, (float)Convert.ToDouble(data));
        }
        else
        {
            throw new Exception("Unsupported type");
        }
    }
    /// <summary>
    /// Loads a string, int or float.
    /// </summary>
    /// <typeparam name="T">The type to load (string, int or float).</typeparam>
    /// <param name="dataName">The filename.</param>
    /// <param name="data">The defult data to save if the file doesn't exist.</param>
    public static T Load<T>(string dataName, T defaultValue = default, SaveMode saveMode = SaveMode.Slot)
    {
        dataName = (saveMode != SaveMode.Global ? SaveSlot.ToString() : "") + dataName;
        Type selectedType = typeof(T);
        if (selectedType == typeof(string))
        {
            if (defaultValue != default)
            {
                return (T)Convert.ChangeType(PlayerPrefs.GetString(dataName, defaultValue.ToString()), typeof(T));
            }
            return (T)Convert.ChangeType(PlayerPrefs.GetString(dataName), typeof(T));
        }
        else if (selectedType == typeof(int))
        {
            if (defaultValue != default)
            {
                return (T)Convert.ChangeType(PlayerPrefs.GetInt(dataName, Convert.ToInt32(defaultValue)), typeof(T));
            }
            return (T)Convert.ChangeType(PlayerPrefs.GetInt(dataName), typeof(T));
        }
        else if (selectedType == typeof(float))
        {
            if (defaultValue != default)
            {
                return (T)Convert.ChangeType(PlayerPrefs.GetFloat(dataName, (float)Convert.ToDouble(defaultValue)), typeof(T));
            }
            return (T)Convert.ChangeType(PlayerPrefs.GetFloat(dataName), typeof(T));
        }
        else
        {
            throw new Exception("Unsupported type");
        }
    }
    /// <summary>
    /// Appeands a string, int or float.
    /// </summary>
    /// <typeparam name="T">The type to appeand (string, int or float).</typeparam>
    /// <param name="dataName">The filename.</param>
    /// <param name="data">The data to appeand.</param>
    public static void Appeand<T>(string dataName, T data, SaveMode saveMode = SaveMode.Slot)
    {
        Type selectedType = typeof(T);
        if (selectedType == typeof(string))
        {
            Save(dataName, Load<T>(dataName).ToString() + data);
        }
        else if (selectedType == typeof(int))
        {
            Save(dataName, Convert.ToInt32(Load<T>(dataName)) + Convert.ToInt32(data));
        }
        else if (selectedType == typeof(float))
        {
            Save(dataName, (float)Convert.ToDouble(Load<T>(dataName)) + (float)Convert.ToDouble(data));
        }
        else
        {
            throw new Exception("Unsupported type");
        }
    }
    /// <summary>
    /// Returns whether key dataName exists.
    /// </summary>
    /// <param name="dataName">The name of the key.</param>
    /// <param name="saveMode">The save mode</param>
    /// <returns></returns>
    public static bool HasKey(string dataName, SaveMode saveMode = SaveMode.Slot)
    {
        dataName = (saveMode != SaveMode.Global ? SaveSlot.ToString() : "") + dataName;
        return PlayerPrefs.HasKey(dataName);
    }
}
