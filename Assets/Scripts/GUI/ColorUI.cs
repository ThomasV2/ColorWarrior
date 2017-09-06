using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ColorUI : MonoBehaviour {

    [SerializeField]
    private Image[] colors;

    public void PrintColor(Color[] newColors, int[] colorIdx)
    {
        for (int i = 0; i < 4; ++i)
        {
            colors[i].color = newColors[i];
            if (ColorWarriorInput.IsConnected())
            {
                ColorWarriorInput.SetButtonColor(i, colorIdx[i]);
            }
        }
    }
}
