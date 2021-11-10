using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAduioSetting : UIWindow
{
    
    //public AuidoPreafabSO Config;
    public Image musicOff;
    public Image soundOff;

    public Toggle toggleMusic;
    public Toggle toggleSound;

    public Slider sliderMusic;
    public Slider sliderSound;
    // Start is called before the first frame update
    void Start()
    {
        musicOff.enabled = !Config.MusicOn;
        soundOff.enabled = !Config.SoundOn;
        this.toggleMusic.isOn = Config.MusicOn;
        this.toggleSound.isOn = Config.SoundOn;
        this.sliderMusic.value = Config.MusicVolume;
        this.sliderSound.value = Config.SoundVolume;
        this.sliderMusic.onValueChanged.AddListener(MusicVolume);
        this.sliderSound.onValueChanged.AddListener(SoundVolume);
        this.toggleMusic.onValueChanged.AddListener(MusicToogle);
        this.toggleSound.onValueChanged.AddListener(SfxToogle);
    }

    public override void OnCloseClick()
    {
        this.sliderMusic.onValueChanged.RemoveListener(MusicVolume);
        this.sliderSound.onValueChanged.RemoveListener(SoundVolume);
        this.toggleMusic.onValueChanged.RemoveListener(MusicToogle);
        this.toggleSound.onValueChanged.RemoveListener(SfxToogle);
        base.OnCloseClick();
    }
    public override void OnYesClick()
    {
        AudioManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        PlayerPrefs.Save();
        base.OnYesClick();
    }
    // Update is called once per frame
    public void MusicToogle(bool on)
    {
        Debug.LogFormat("MusicToogle:{0}", on);
        musicOff.enabled = !on;
        Config.MusicOn = on;
        //AudioManager.Instance.MusicOn = on;
        AudioManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
    }

    public void SfxToogle(bool on)
    {
        soundOff.enabled = !on;
        Config.SoundOn = on;
        AudioManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
    }

    public void MusicVolume(float vol)
    {
        Debug.LogFormat("MusicVolume:{0}", vol);
        Config.MusicVolume = (int)vol;
        PlaySound();
    }

    public void SoundVolume(float vol)
    {
        Config.SoundVolume = (int)vol;
        PlaySound();
    }

    float lastPlay = 0;
    private void PlaySound()
    {
        if(Time.realtimeSinceStartup - lastPlay > 0.1)
        {
            lastPlay = Time.realtimeSinceStartup;
            AudioManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        }
    }
}
