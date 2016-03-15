using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public static class PlayerPrefsUtils
{
    private static readonly string Music = "music";
    private static readonly string Sound = "sound";
    private static readonly char SplitChar = ';';

    #region public methods
    //获取音乐状态
    public static bool GetMusicState()
    {
        return GetBool(Music);
    }

    //保存音乐状态
    public static void SaveMusicState(bool opened)
    {
        SetBool(Music, opened);
    }

    //获取音效状态
    public static bool GetSoundState()
    {
        return GetBool(Sound);
    }

    //保存音效状态
    public static void SaveSoundState(bool opened)
    {
        SetBool(Sound, opened);
    }
    #endregion


    #region private methods
    private static bool GetBool(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetInt(key) == 1;
        }
        return true;
    }

    private static void  SetBool(string key,bool state)
    {
        PlayerPrefs.SetInt(key, state ? 1 : 0);
        Save();
    }

    private static void Save()
    {
        PlayerPrefs.Save();
    }
    #endregion
}
