using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoSingleton<AudioManager>
{
    public AudioMixer audioMixer;
    public AudioSource musicAudioSource;
    public AudioSource sfxAduioSource;
    
    const string MusicPath = "Music/";
    const string SfxPath = "Sound/";
    private bool musicOn;
    public bool MusicOn 
    {
        get { return musicOn; }
        set
        {
            musicOn = value;
            this.MusicMute(!musicOn);
        } 
    }
    private bool soundOn;
    public bool SoundOn 
    { 
        get { return soundOn; }
        set
        {
            soundOn = value;
            this.SoundMute(!soundOn);
        } 
    }

   

    private int musicVolume;
    public int MusicVolume
    {
        get { return musicVolume; }
        set
        {
            musicVolume = value;
            if (musicOn) this.SetVolume("MusicVolume", musicVolume);
        }
    }
    private int soundVolume;
    public int SoundVolume
    {
        get { return soundVolume; }
        set
        {
            soundVolume = value;
            if (soundOn) this.SetVolume("SfxVolume", soundVolume);
        }
    }
    private void Start()
    {
        this.MusicOn = Config.MusicOn;
        this.SoundOn = Config.SoundOn;
        this.MusicVolume = Config.MusicVolume;
        this.SoundVolume = Config.SoundVolume;
    }

    
    public void Init()
    {

    }

    private void MusicMute(bool mute)
    {
        //this.SetVolume("MusicVolume", mute ? -10 : musicVolume);
        this.musicAudioSource.mute = mute;
    }

    
    private void SoundMute(bool mute)
    {
        //this.SetVolume("SfxVolume", mute ? -10 : musicVolume);
        this.sfxAduioSource.mute = mute;
    }

    private void SetVolume(string name, int value)
    {
        float volume = value * 0.5f - 60f;
        Debug.LogFormat("SetVolume:name:{0},value:{1}", name,volume);
        this.audioMixer.SetFloat(name, value);
    }

    public void PlayMusic(string name)
    {
        AudioClip clip = Resloader.Load<AudioClip>(MusicPath + name);
        if (clip == null)
        {
            Debug.LogWarningFormat("PlayMusic:{0} not existed.", name);
            return;
        }
        if (musicAudioSource.isPlaying)
        {
            musicAudioSource.Stop();
        }
        musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }

    public void PlaySound(string name)
    {
        AudioClip clip = Resloader.Load<AudioClip>(SfxPath + name);
        if (clip == null)
        {
            Debug.LogWarningFormat("PlaySound:{0} not existed.", name);
            return;
        }

        sfxAduioSource.PlayOneShot(clip);
    }

}
