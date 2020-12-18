using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pause : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> pauseList;
    [SerializeField] private List<GameObject> settingList;
    [SerializeField] private List<TMP_Text> gameOverList;
    [SerializeField] private GameObject transitionUI;
    private int currentPauseChoice = 0;

    public void UpdatePauseAction(int index)
    {
        for (int i = 0; i < pauseList.Count; i++)
        {
            if (i == index)
            {
                pauseList[i].color = Color.blue;
            }
            else
            {
                pauseList[i].color = Color.white;
            }
        }
    }

    public void UpdateSettingAction(int index)
    {
        for (int i = 0; i < settingList.Count; i++)
        {
            if (i == index)
            {
                settingList[i].GetComponentInChildren<TMP_Text>().color = Color.blue;
            }
            else
            {
                settingList[i].GetComponentInChildren<TMP_Text>().color = Color.black;
            }
        }
    }

    public void UpdateGameOverAction(int index)
    {
        for (int i = 0; i < gameOverList.Count; i++)
        {
            if (i == index)
            {
                gameOverList[i].color = Color.blue;
            }
            else
            {
                gameOverList[i].color = Color.white;
            }
        }
    }
}
