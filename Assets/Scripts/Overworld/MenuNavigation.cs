using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MenuNavigation : MonoBehaviour {

    public enum MenuState
    {
        HERO,
        MENU,
        SUBMENU
    };

    [System.Serializable]
    class MenuPanel
    {
        public GameObject menu = null;
        public GameObject firstButton = null;
    }

    [SerializeField]
    private OWHeroController hero;
    [SerializeField]
    EventSystemPatch eventSystem;
    [SerializeField]
    private List<Button> mainMenuButtons;
    [SerializeField]
    private List<MenuPanel> menus;
    [SerializeField]
    private GameObject MainBtns;
    [SerializeField]
    private GameObject OptionBtns;

    private int state;
    private InputsManager inputsManager;

    void Start()
    {
        inputsManager = FindObjectOfType<InputsManager>();
        ColorWarriorInput.SetButtonColor(0, 2);
        ColorWarriorInput.SetButtonColor(1, 1);
        ColorWarriorInput.SetButtonColor(2, 3);
        ColorWarriorInput.SetButtonColor(3, 2);
        state = (int)MenuState.HERO;
    }

    void Update()
    {
        if (inputsManager.GetButtonDown("Jump"))
        {
            state = (state == 0 ? 1 : state - 1);
            switch (state)
            {
                case (int)MenuState.HERO:
                    SwapToHero();
                    break;
                case (int)MenuState.MENU:
                    OpenMenu((int)MenuState.MENU);
                    break;

            }
        }
        if (Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void OpenMenu(int menu)
    {
        int idx = menu - 1;
        state = (menu > (int)MenuState.SUBMENU ? (int)MenuState.SUBMENU : menu);
        foreach (Button button in mainMenuButtons)
        {
            button.interactable = (menu == (int)MenuState.MENU);
        }
        hero.SetActive(false);
        eventSystem.UpdateEventSystem(menus[idx].firstButton);
        foreach (MenuPanel m in menus)
        {
            if (m.menu != null)
            {
                m.menu.SetActive((m.menu == menus[idx].menu));
            }
        }
    }

    public void SwapToHero()
    {
        hero.SetActive(true);
        eventSystem.ResetEventSystem();
    }

    public void GoToOptions()
    {
        MainBtns.SetActive(false);
        OptionBtns.SetActive(true);
        eventSystem.UpdateEventSystem(OptionBtns.transform.GetChild(0).gameObject);
    }

    public void FromOption()
    {
        OptionBtns.SetActive(false);
        MainBtns.SetActive(true);
        eventSystem.UpdateEventSystem(MainBtns.transform.GetChild(0).gameObject);
    }
}
