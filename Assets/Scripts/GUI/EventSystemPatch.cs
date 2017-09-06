using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class EventSystemPatch : MonoBehaviour {

    [SerializeField]
    private EventSystem eventSys;

    public void UpdateEventSystem(GameObject obj = null)
    {
        if (obj != null)
            eventSys.firstSelectedGameObject = obj;
        StartCoroutine(Refresh());
    }

    public void ResetEventSystem()
    {
        eventSys.firstSelectedGameObject = null;
        StartCoroutine(Refresh());
    }

    IEnumerator Refresh()
    {
        eventSys.enabled = false;
        yield return null;
        eventSys.enabled = true;
    }
}
