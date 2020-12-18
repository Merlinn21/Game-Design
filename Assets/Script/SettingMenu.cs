using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public GameObject[] volBars;
    public GameObject[] sfxBars;

    private float volCheck;
    private int musicVol;
    private int sfxVol;

    private int currentMusicVol;
    private int currentSfxVol;

    private void Start()
    {
        SetVolume(getMusicVol());
        SetSfx(getSfxVol());
    }

    public int getMusicVol()
    {
        musicVol = PlayerPrefs.GetInt("Music_Volume");
        Debug.Log(musicVol);
        return musicVol;
    }

    public int getSfxVol()
    {
        sfxVol = PlayerPrefs.GetInt("SFX_Volume");
        return sfxVol;
    }

    public int getCurrentMusicVol()
    {
        return currentMusicVol;
    }

    public int getCurrentSfxVol()
    {
        return currentSfxVol;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("music",volume);
        currentMusicVol = (int)volume;
        VolBar();
    }

    public void SetSfx(float sfx)
    {
        audioMixer.SetFloat("sfx", sfx);
        currentSfxVol = (int)sfx;
        SfxBar();
    }

    public void VolBar()
    {
        audioMixer.GetFloat("music", out volCheck);
        int index;
        int x = 4;

        while (x >= 0)
        {
            volBars[x].GetComponent<Image>().color = Color.white;
            x--;
        }

        switch (volCheck)
        {
            case -10:
                index = 4;
                break;
            case -15:
                index = 3;
                break;
            case -20:
                index = 2;
                break;
            case -25:
                index = 1;
                break;
            case -30:
                index = 0;
                break;
            default:
                index = 4;
                break;
        }
        
        while(index >= 0)
        {
            volBars[index].GetComponent<Image>().color = Color.green;
            index--;
        }
    }
    public void SfxBar()
    {
        audioMixer.GetFloat("sfx", out volCheck);
        int index;
        int x = 4;

        while (x >= 0)
        {
            sfxBars[x].GetComponent<Image>().color = Color.white;
            x--;
        }

        switch (volCheck)
        {
            case -10:
                index = 4;
                break;
            case -15:
                index = 3;
                break;
            case -20:
                index = 2;
                break;
            case -25:
                index = 1;
                break;
            case -30:
                index = 0;
                break;
            default:
                index = 4;
                break;
        }

        while (index >= 0)
        {
            sfxBars[index].GetComponent<Image>().color = Color.green;
            index--;
        }
    }
}
