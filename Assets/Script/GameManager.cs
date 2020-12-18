using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum GameState
{
    FreeRoam,
    Battle,
    Transition,
    Dialogue,
    Setting,
    Status,
    Menu,
    GameOver
}
public class GameManager : MonoBehaviour
{
    [Header("UI GAME OBJECT")]
    [SerializeField] private GameObject battleTransition;
    [SerializeField] private GameObject statWindow;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject transitionUI;
    [SerializeField] private GameObject gameOverPanel;

    [Space]
    [Header("Audio")]
    [SerializeField] private AudioSource exploreAudio;
    [SerializeField] private AudioSource battleAudio;
    [SerializeField] private AudioSource uiAudioSource;
    [SerializeField] private AudioClip menuSFX;
    [SerializeField] private AudioClip confirmSFX;

    [Space]
    [Header("Other Script")]
    [SerializeField] private GridMovement playerMove;
    [SerializeField] private BattleSystem battleSystem;
    [SerializeField] private DialogueTrigger dialogueTrigger;
    [SerializeField] private Dialogue startDialogue;
    [SerializeField] private SettingMenu setting;
    [SerializeField] private SceneTransition scene;

    [Space]
    [Header("GameObject")]
    [SerializeField] private Camera mainCamera;
    public GameState state;

    private KeyCode confirmBtn = KeyCode.Z;
    private KeyCode backBtn = KeyCode.X;
    private KeyCode escBtn = KeyCode.Escape;
    private KeyCode statBtn = KeyCode.S;

    private KeyCode up = KeyCode.UpArrow;
    private KeyCode down = KeyCode.DownArrow;
    private int currentPauseChoice = 0;
    private int currentSettingChoice = 0;
    private int currentGameOverChoice = 0;
    public Pause pause;

    int saveMusicVol;
    int saveSfxVol;
    AudioScript audioScript = new AudioScript();



    private void Start()
    {
        playerMove.onEncounter += StartBattle; // manggil fungsi StartBattle kalo onEncounter kepanggil
        battleSystem.onBattleOver += EndBattle;
        playerMove.onDialogue += StartDialogue;
        dialogueTrigger.onDialogueOver += EndDialogue;


        StartCoroutine(audioScript.FadeIn(exploreAudio, 0.5f));
        battleTransition.SetActive(false);
        StartCoroutine(FirstDialogue());
    }

    IEnumerator FirstDialogue()
    {
        yield return new WaitForSeconds(1.5f);
        StartDialogue(startDialogue);
    }

    public void StartBattle(bool randomBattle)
    {
        battleTransition.SetActive(true);

        state = GameState.Battle;       
        battleSystem.gameObject.SetActive(true);
        battleSystem.transform.GetChild(0).gameObject.SetActive(true);
        battleSystem.transform.GetChild(1).gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        battleSystem.StartBattle(randomBattle);
    }
 
    public void EndBattle(bool win , int exp)
    {
        transitionUI.SetActive(false);
        transitionUI.SetActive(true);
        AudioScript audioScript = new AudioScript();
        StartCoroutine(audioScript.FadeOut(battleAudio, 0.3f));
        StartCoroutine(audioScript.FadeIn(exploreAudio, 0.3f));
        battleTransition.SetActive(false);
        battleSystem.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
        state = GameState.FreeRoam;
        if (win)
        {
            //TODO: SHOW EXP PROGRESS

            PlayerStat.exp += exp;

            if(PlayerStat.exp >= PlayerStat.maxExp)
            {
                state = GameState.Status;
                LevelUp();
                PlayerStat.maxExp += 15;
                PlayerStat.exp = 0;
                OpenStat();
            }      
        }
        else
        {
            //Game Over
            gameOverPanel.SetActive(true);
            state = GameState.GameOver;        
        }
    }

    private void LevelUp()
    {
        PlayerStat.lvl += 1;
        PlayerStat.atk += 3;
        PlayerStat.def += 3;
        PlayerStat.satk += 3;
        PlayerStat.sdef += 3;
        PlayerStat.maxMana += 3;
        PlayerStat.maxHealth += 3;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        state = GameState.Dialogue;
        dialogueTrigger.SetDialogue(dialogue);
        dialogueTrigger.StartDialogue();
    }

    public void EndDialogue(bool freeRoam, GhostParty ghostParty) 
    {
        if (freeRoam)
        {
            state = GameState.FreeRoam;
            playerMove.onEventFalse();
        }
        else
        {
            battleTransition.SetActive(true);
            state = GameState.Battle;
            battleSystem.gameObject.SetActive(true);
            battleSystem.transform.GetChild(0).gameObject.SetActive(true);
            battleSystem.transform.GetChild(1).gameObject.SetActive(true);
            mainCamera.gameObject.SetActive(false);
            playerMove.onEventFalse();
            battleSystem.StartEventBattle(ghostParty);
        }
    }


    private void Update()
    {
        switch (state)
        {
            case GameState.FreeRoam:
                playerMove.HandleUpdate();

                if (Input.GetKeyDown(escBtn))
                {
                    //Pause
                    state = GameState.Menu;
                    pausePanel.SetActive(true);
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    state = GameState.Status;
                    OpenStat();
                }
                break;
            case GameState.Dialogue:
                dialogueTrigger.HandleUpdate();
                break;
            case GameState.Status:
                if (Input.GetKeyDown(KeyCode.X))
                {
                    state = GameState.FreeRoam;
                    statWindow.SetActive(false);
                }
                break;
            case GameState.Menu:
                HandlePauseUpdate();
                break;
            case GameState.Setting:
                HandleSetting();
                break;
            case GameState.GameOver:
                gameOverPanel.SetActive(true);
                HandleGameOver();
                break;

        }
    }

    private void OpenStat()
    {
        statWindow.SetActive(true);
        PlayerStatWindow window = statWindow.gameObject.GetComponent<PlayerStatWindow>();
        window.UpdateUI();
    }

    private void HandlePauseUpdate()
    {
        if (Input.GetKeyDown(down))
        {
            if (currentPauseChoice < 2)
                currentPauseChoice++;
            uiAudioSource.PlayOneShot(menuSFX);
        }
        else if (Input.GetKeyDown(up))
        {
            if (currentPauseChoice > 0)
                currentPauseChoice--;
            uiAudioSource.PlayOneShot(menuSFX);

        }
        pause.UpdatePauseAction(currentPauseChoice);

        if (Input.GetKeyDown(confirmBtn))
        {
            uiAudioSource.PlayOneShot(confirmSFX);
            if (currentPauseChoice == 0)
            {
                //RESUME
                pausePanel.SetActive(false);
                state = GameState.FreeRoam;
            }
            else if (currentPauseChoice == 1)
            {
                //TODO: Open Setting
                state = GameState.Setting;
                settingPanel.SetActive(true);
                transitionUI.GetComponent<Image>().enabled = false;
                
            }
            else if (currentPauseChoice == 2)
            {
                Application.Quit();
            }
        }
    }

    private void HandleSetting()
    {
        if (Input.GetKeyDown(down))
        {
            if (currentSettingChoice < 1)
                currentSettingChoice++;
            uiAudioSource.PlayOneShot(menuSFX);
        }
        else if (Input.GetKeyDown(up))
        {
            if (currentSettingChoice > 0)
                currentSettingChoice--;
            uiAudioSource.PlayOneShot(menuSFX);

        }

        pause.UpdateSettingAction(currentSettingChoice);
        if (Input.GetKeyDown(backBtn))
        {
            //TODO: Close Setting
            settingPanel.SetActive(false);
            transitionUI.GetComponent<Image>().enabled = true;
            state = GameState.Menu;
        }
        else if (Input.GetKeyDown(confirmBtn))
        {
            if(currentSettingChoice == 0)
            {
                saveMusicVol = setting.getCurrentMusicVol();
                saveSfxVol = setting.getCurrentSfxVol();
                PlayerPrefs.SetInt("Music_Volume", saveMusicVol);
                PlayerPrefs.SetInt("SFX_Volume", saveSfxVol);
                PlayerPrefs.Save();
                settingPanel.SetActive(false);
                transitionUI.GetComponent<Image>().enabled = true;
                state = GameState.Menu;
            }
            else
            {
                setting.SetVolume(saveMusicVol);
                setting.SetSfx(saveSfxVol);
                settingPanel.SetActive(false);
                transitionUI.GetComponent<Image>().enabled = true;
                state = GameState.Menu;
            }
        }
    }

    private void HandleGameOver()
    {
        if (Input.GetKeyDown(down))
        {
            if (currentGameOverChoice < 2)
                currentGameOverChoice++;
            uiAudioSource.PlayOneShot(menuSFX);
        }
        else if (Input.GetKeyDown(up))
        {
            if (currentGameOverChoice > 0)
                currentGameOverChoice--;
            uiAudioSource.PlayOneShot(menuSFX);

        }

        pause.UpdateGameOverAction(currentGameOverChoice);

        if (Input.GetKeyDown(confirmBtn))
        {
            if(currentGameOverChoice == 0)
            {
                //Retry
                StartCoroutine(scene.LoadNextScene(SceneManager.GetActiveScene().name));
            }
            else if(currentGameOverChoice == 1)
            {
                //Edxit
                Application.Quit();
            }
        }
    }
}
