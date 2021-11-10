using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Config
{
  public static bool MusicOn
    {
        get
        {
            return PlayerPrefs.GetInt("Music", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("Music", value ? 1 : 0);
            AudioManager.Instance.MusicOn = value;
        }
    }

    public static bool SoundOn
    {
        get
        {
            return PlayerPrefs.GetInt("Sound", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("Sound", value ? 1 : 0);
            AudioManager.Instance.SoundOn = value;
        }
    }

    public static int MusicVolume
    {
        get
        {
            return PlayerPrefs.GetInt("MusicVolume", 10);
        }
        set
        {
            PlayerPrefs.SetInt("MusicVolume", value );
            AudioManager.Instance.MusicVolume = value;
        }
    }

    public static int SoundVolume
    {
        get
        {
            return PlayerPrefs.GetInt("SoundVolume", 10);
        }
        set
        {
            PlayerPrefs.SetInt("SoundVolume", value);
            AudioManager.Instance.SoundVolume = value;
        }
    }

    Config()
    {
        PlayerPrefs.Save();
    }
}