using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum MenuState
{
    MainMenu,
    Setting,
    Busy
}
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private List<Button> buttonList;
    [SerializeField] SceneTransition scene;
    [SerializeField] private GameObject settingObject;
    [SerializeField] private GameObject transitionObj;

    private KeyCode up = KeyCode.UpArrow;
    private KeyCode down = KeyCode.DownArrow;

    public KeyCode confirmButton = KeyCode.F;
    public KeyCode backButton = KeyCode.X;

    private int currentMultiChoice = 0;

    public string nextSceneName;
    public AudioSource menuSfx;
    public AudioClip menuNavigate;
    public AudioClip enterGame;
    private MenuState state = MenuState.MainMenu;

    private void Update()
    {
        if (state == MenuState.MainMenu)
            HandleMain();
        else if (state == MenuState.Setting)
            HandleSetting();
        
    }

    private void HandleMain()
    {
        if (Input.GetKeyDown(down) && state == MenuState.MainMenu)
        {
            if (currentMultiChoice < 2)
                currentMultiChoice++;
            menuSfx.PlayOneShot(menuNavigate);
        }
        else if (Input.GetKeyDown(up) && state == MenuState.MainMenu)
        {
            if (currentMultiChoice > 0)
                currentMultiChoice--;
            menuSfx.PlayOneShot(menuNavigate);
        }
        UpdateChooseMenu(currentMultiChoice);

        if (Input.GetKeyDown(confirmButton))
        {
            if (currentMultiChoice == 0)
            {
                menuSfx.PlayOneShot(enterGame);
                state = MenuState.Busy;
                StartCoroutine(scene.LoadNextScene(nextSceneName));
            }
            else if (currentMultiChoice == 1)
            {
                //TODO: Open Setting
                state = MenuState.Setting;
                transitionObj.SetActive(false);
                settingObject.SetActive(true);
            }
            else if (currentMultiChoice == 2)
            {
                Application.Quit();
            }
        }
    }

    private void HandleSetting()
    {
        if (Input.GetKeyDown(backButton))
        {
            //TODO: Close Setting
            transitionObj.SetActive(true);
            settingObject.SetActive(false);
            state = MenuState.MainMenu;
        }
    }


    public void UpdateChooseMenu(int index)
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            if (i == index)
            {
                buttonList[i].GetComponentInChildren<TMP_Text>().color = Color.blue;
            }
            else
            {
                buttonList[i].GetComponentInChildren<TMP_Text>().color = Color.black;
            }
        }
    }

}
