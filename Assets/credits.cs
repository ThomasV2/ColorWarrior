using UnityEngine;
using System.Collections;

public class credits : MonoBehaviour {

    [SerializeField]
    private MainMenuUI gui;
    [SerializeField]
    private Animator anim;

    public void CloseMe()
    {
        anim.SetTrigger("Close");
    }

    public void EndClose()
    {
        gui.CloseCredit();
    }

}
