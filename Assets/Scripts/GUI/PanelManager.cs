using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour {

    [SerializeField]
    private EventSystemPatch eventSys;
    //pause panel link
    [SerializeField]
    private GameObject PausePanel;
    //first btn in pause
    [SerializeField]
    private GameObject btnPauseStart;
    //first btn in the options
    [SerializeField]
    private GameObject btnOptStart;
    //death panel link
    [SerializeField]
    private GameObject DeathPanel;
    //first btn in death panel
    [SerializeField]
    private GameObject btnDie;
    //win panel link
    [SerializeField]
    private GameObject WinPanel;
    //first btn in winPanel
    [SerializeField]
    private GameObject btnWin;
    //Next level button (to disable if last level)
    [SerializeField]
    private Button nextLevel;


    private bool isGamePaused;
    private GameManager gameManager;

    void Start()
    {
        if (eventSys == null)
        {
            eventSys = GameObject.FindObjectOfType<EventSystemPatch>();
        }
        if (ApplicationDatas.Game.CurrentLevel == ApplicationDatas.MAX_LEVEL - 1)
        {
            nextLevel.interactable = false;
        }
        gameManager = FindObjectOfType<GameManager>();
        isGamePaused = false;
    }

    public void GoToWorldMap()
    {
        //faire ce qu'il faut pour le faire quitter
        //(lui faire perdre un peut de shards ou ceux gagné à la run ou ceux avant le dernier checkpoint)
        isGamePaused = false;
        PausePanel.SetActive(false);
        Debug.Log("Quit");
        SceneManager.LoadScene("Overworld");
        Time.timeScale = 1;
    }

    public void RetryLevel()
    {
        isGamePaused = false;
        PausePanel.SetActive(false);
        Debug.Log("Retry");
        SceneManager.LoadScene("Levels");
        Time.timeScale = 1;
    }

    public void NextLevel()
    {
        isGamePaused = false;
        if (ApplicationDatas.Game.CurrentLevel < ApplicationDatas.MAX_LEVEL - 1)
        {
            ApplicationDatas.Game.CurrentLevel++;
        }
        SceneManager.LoadScene("Levels");
        Time.timeScale = 1;
    }

    public void GoToOptions()
    {

        //eventSys.UpdateEventSystem(btnOptStart);
    }

    public void FromOptions()
    {

        //eventSys.UpdateEventSystem(btnPauseStart);
    }

    public void Resume()
    {
        FindObjectOfType<HeroController>().Resume();
        isGamePaused = false;
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    void LateUpdate()
    {
        if (!gameManager.LevelEnded() && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused == false)
            {
                isGamePaused = true;
                Time.timeScale = 0;
                PausePanel.SetActive(true);
                eventSys.UpdateEventSystem(btnPauseStart);
                FindObjectOfType<HeroController>().Pause();
            }
            else
            {
                isGamePaused = false;
                PausePanel.SetActive(false);
                Time.timeScale = 1;
                FindObjectOfType<HeroController>().Resume();
            }
        }
        /*if (Input.GetKeyDown(KeyCode.D))
        {
            DeathUI();
        }*/
    }

    public void DeathUI()
    {
        DeathPanel.SetActive(true);
        DeathPanel.GetComponent<Animator>().SetTrigger("Die");
        eventSys.UpdateEventSystem(btnDie);
    }

    public void WinUI()
    {
        WinPanel.SetActive(true);
        WinPanel.GetComponent<Animator>().SetTrigger("Win");
        eventSys.UpdateEventSystem(btnWin);
    }

}
