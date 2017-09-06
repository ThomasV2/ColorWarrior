using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour {

    [SerializeField]
    private GameObject MainBtns;
    [SerializeField]
    private GameObject OptionBtns;
    [SerializeField]
    private Button Load;
    [SerializeField]
    private EventSystemPatch eventSys;
    [SerializeField]
    private Toggle soundTog;
    [SerializeField]
    private Toggle musicTog;
    [SerializeField]
    private GameObject CreditPanel;
    [SerializeField]
    private Color btnLocked;
    [SerializeField]
    private Color btnUnlocked;

    void Start()
    {
        SetupLoad();
        int val = 1;
        if (PlayerPrefs.HasKey("IsMusic"))
            val = PlayerPrefs.GetInt("IsMusic");
        if (val == 0)
            musicTog.isOn = true;

        val = 1;
        if (PlayerPrefs.HasKey("IsSound"))
            val = PlayerPrefs.GetInt("IsSound");
        if (val == 0)
            soundTog.isOn = true;
    }

    private void SetupLoad()
    {
        Load.interactable = ApplicationDatas.HasSave();
        if (Load.interactable)
        {
            Load.transform.FindChild("Text").GetComponent<Text>().color = btnUnlocked;
        }
        else
        {
            Load.transform.FindChild("Text").GetComponent<Text>().color = btnLocked;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void NewGame()
    {
        ApplicationDatas.NewGame();
        SceneManager.LoadScene("Overworld");
    }

    public void LoadGame()
    {
        ApplicationDatas.Load();
        SceneManager.LoadScene("Overworld");
    }

    public void ClearDatas()
    {
        ApplicationDatas.Clear();
        SetupLoad();
        eventSys.UpdateEventSystem();
    }

    public void GoToOptions()
    {
        MainBtns.SetActive(false);
        OptionBtns.SetActive(true);
        eventSys.UpdateEventSystem(OptionBtns.transform.GetChild(0).gameObject);
    }

    public void FromOption()
    {
        OptionBtns.SetActive(false);
        MainBtns.SetActive(true);
        eventSys.UpdateEventSystem(MainBtns.transform.GetChild(0).gameObject);
    }

    public void SwitchMusique()
    {
        int val = 1;

        if (PlayerPrefs.HasKey("IsMusic"))
            val = PlayerPrefs.GetInt("IsMusic");
        if (val == 1)
            val = 0;
        else
            val = 1;

        //si val == 0 = pas musique / val == 1 = musique
        PlayerPrefs.SetInt("IsMusic", val);
    }

    public void SwitchSound()
    {
        int val = 1;

        if (PlayerPrefs.HasKey("IsSound"))
            val = PlayerPrefs.GetInt("IsSound");
        if (val == 1)
            val = 0;
        else
            val = 1;

        //si val == 0 = pas bruitage / val == 1 = bruitage
        PlayerPrefs.SetInt("IsSound", val);
    }

    public void LevelMusique(Slider val)
    {
        PlayerPrefs.SetFloat("MusicLevel", val.value);
    }

    public void LevelSound(Slider val)
    {
        PlayerPrefs.SetFloat("SoundLevel", val.value);
    }

    public void OpenCredit()
    {
        MainBtns.SetActive(false);
        CreditPanel.SetActive(true);
        CreditPanel.GetComponent<Animator>().SetTrigger("Open");
        eventSys.UpdateEventSystem(CreditPanel.transform.FindChild("Back").gameObject);
    }

    public void CloseCredit()
    {
        CreditPanel.SetActive(false);
        MainBtns.SetActive(true);
        eventSys.UpdateEventSystem(MainBtns.transform.GetChild(0).gameObject);
    }
}
