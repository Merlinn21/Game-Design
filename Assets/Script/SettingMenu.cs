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

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("music",volume);
        VolBar();
    }

    public void SetSfx(float sfx)
    {
        audioMixer.SetFloat("sfx", sfx);
        SfxBar();
    }

    private void Start()
    {
        //TODO: PlayerPref ambil setting
        VolBar();
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
            case 0:
                index = 4;
                break;
            case -4:
                index = 3;
                break;
            case -8:
                index = 2;
                break;
            case -12:
                index = 1;
                break;
            case -16:
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
            case 0:
                index = 4;
                break;
            case -4:
                index = 3;
                break;
            case -8:
                index = 2;
                break;
            case -12:
                index = 1;
                break;
            case -16:
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
