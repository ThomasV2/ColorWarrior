using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputsManager : MonoBehaviour
{
    private Dictionary<string, int> inputName = new Dictionary<string, int>();

    void Awake()
    {
        ColorWarriorInput.Init();
    }

	// Use this for initialization
	void Start ()
    {
        inputName.Add("Fire1", 0);
        inputName.Add("Jump", 1);
        inputName.Add("Fire2", 2);
        inputName.Add("Fire3", 3);
    }

    void OnDestroy()
    {
        ColorWarriorInput.Close();
    }

    public bool GetButtonDown(string input)
    {
        return (ColorWarriorInput.IsConnected() && ColorWarriorInput.GetButtonDown(inputName[input]) || Input.GetButtonDown(input));
    }

    public bool GetButton(string input)
    {
        return (ColorWarriorInput.IsConnected() && ColorWarriorInput.GetButtonDown(inputName[input]) || Input.GetButton(input));
    }
}
