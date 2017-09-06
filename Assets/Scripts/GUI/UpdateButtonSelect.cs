using UnityEngine;
using System.Collections;

public class UpdateButtonSelect : MonoBehaviour
{
    [SerializeField]
    private GameObject button;
    private EventSystemPatch eventPatch;

    void Start()
    {
        eventPatch = FindObjectOfType<EventSystemPatch>();
    }

	public void SelectButton()
    {
        eventPatch.UpdateEventSystem(button);
    }
}
