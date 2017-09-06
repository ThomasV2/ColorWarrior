using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BonusUI : MonoBehaviour {

    [SerializeField]
    private Image img;
    [SerializeField]
    private Image bonus;

    private float timer;
    private float blinkingTime = 2;
    private float blinkMax = 0.1f;
    //private InfosUI ui;
    private int type;

    public void Init(Sprite sprite, float time, InfosUI infos, Color bg)
    {
        GetComponent<RectTransform>().localRotation = Quaternion.identity;
        timer = time;
        bonus.sprite = sprite;
        img.color = bg;
        StartCoroutine(Decrease());
        //ui = infos;
    }

    public void SetTime(float newTime)
    {
        timer = newTime;
    }

    private IEnumerator Decrease()
    {
        blinkingTime = blinkMax;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 1f && (blinkingTime -= Time.deltaTime) < 0)
            {
                img.enabled = !img.enabled;
                blinkingTime = blinkMax;
            }
            yield return null;
        }
        yield return null;
        //ui.RemoveMe(this.)
        Destroy(this.gameObject);
    }

    protected void OnRectTransformDimensionsChange()
    {
        if (GetComponent<LayoutElement>() != null)
        {
            StartCoroutine(LayoutAdapt());
        }
    }

    IEnumerator LayoutAdapt()
    {
        var rectTransform = GetComponent<RectTransform>();
        int height = (int)rectTransform.rect.height;
        GetComponent<LayoutElement>().minWidth = (height);
        GetComponent<LayoutElement>().enabled = false;
        yield return null;
        GetComponent<LayoutElement>().enabled = true;
        Debug.Log("Adapt");
    }
}
