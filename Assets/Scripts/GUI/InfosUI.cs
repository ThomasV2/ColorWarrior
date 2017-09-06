using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InfosUI : MonoBehaviour {

    [SerializeField]
    private GameObject[] lifes;
    [SerializeField]
    private GameObject bonusUiPrefab;
    [SerializeField]
    private GameObject grid;
    [SerializeField]
    private Dictionary<int, BonusUI> bonusList;

    private int maxLife;

    public void LoseALife(int val)
    {
        if (val >= 0)
        {
            lifes[val].GetComponent<Animator>().SetTrigger("LoseLife");
        }
        else
            Debug.LogWarning("I can't acces !");
    }

    public void WinALife(int index)
    {
        if (index < maxLife)
        {
            lifes[index].GetComponent<Animator>().SetTrigger("WinLife");
        }
        else
            Debug.LogWarning("already all lifes");
    }

    public void AddBonus(int type, Sprite img, float time, Color col)
    {
        if (time > 0.0f)
        {
            //si le bonus existe déjà, add du temps à l'image
            if (bonusList.ContainsKey(type) && bonusList[type] != null)
            {
                BonusUI obj = bonusList[type];
                obj.SetTime(time);
            }
            else
            {
                if (bonusList.ContainsKey(type) && bonusList[type] == null)
                    bonusList.Remove(type);
                GameObject tmp;
                tmp = Instantiate(bonusUiPrefab);
                tmp.transform.SetParent(grid.transform);
                tmp.GetComponent<BonusUI>().Init(img, time, this, col);
                bonusList.Add(type, tmp.GetComponent<BonusUI>());
            }
        }
    }

    public void InitLife(int value)
    {
        maxLife = value;
        for (int i = 0; i < maxLife; ++i)
        {
            lifes[i].GetComponent<Animator>().SetTrigger("WinLife");
        }
        for (int i = maxLife; i < lifes.Length; ++i)
        {
            lifes[i].transform.parent.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        maxLife = lifes.Length;
        bonusList = new Dictionary<int, BonusUI>();
    }
}
