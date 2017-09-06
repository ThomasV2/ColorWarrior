using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuSwapColor : MonoBehaviour
{
    List<int> defaultColors = new List<int>();
    float timer;

	// Use this for initialization
	void Start ()
    {
        defaultColors.Add(0);
        defaultColors.Add(1);
        defaultColors.Add(2);
        defaultColors.Add(3);
        ShuffleColors();
        timer = 0;
    }

    // Update is called once per frame
    void Update ()
    {
        if ((timer += Time.deltaTime) > 1)
        {
            ShuffleColors();
            timer = 0;
            Debug.LogError("shuffle");
        }
	}

    void ShuffleColors()
    {
        if (ColorWarriorInput.IsConnected())
        {
            for (int i = 0; i < 4; ++i)
            {
                if (--defaultColors[i] < 0)
                {
                    defaultColors[i] = 3;
                }
                ColorWarriorInput.SetButtonColor(i, defaultColors[i]);
            }
        }
        else
            Debug.LogError("not connected");
    }
    //kmu_2   kmu12345
}
