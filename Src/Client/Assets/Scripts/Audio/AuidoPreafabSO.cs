using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Config", fileName = "AudioConfig")]
public class AuidoPreafabSO : ScriptableObject
{
    public bool MusicOn { get; internal set; }
    public bool SoundOn { get; internal set; }
    public float MusicVolume { get; internal set; }
    public float SoundVolume { get; internal set; }

    internal void Save()
    {
        throw new NotImplementedException();
    }
}
