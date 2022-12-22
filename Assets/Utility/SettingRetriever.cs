using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingRetriever
{
    public static bool getSetting(string key)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.SetInt(key, 0);
        }

        return PlayerPrefs.GetInt(key) == 1 ? true : false;
    }
}
