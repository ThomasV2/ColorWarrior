using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private Color[] availableColors;
    [SerializeField]
    private ColorUI colorUI;
    [SerializeField]
    private List<Image> buttons;
    [SerializeField]
    private Material multicolor;
    [SerializeField]
    private Text shardsDisplay;
    [SerializeField]
    private GameObject gameOverScreen;
    [SerializeField]
    private GameObject victoryScreen;

    private List<AKillable> entityList;
    private int shardsCount;
    private bool levelEnded;
    private int[] uiColors = new int[4];

    void Awake()
    {
        entityList = new List<AKillable>();
        if (colorUI == null)
            colorUI = GameObject.FindObjectOfType<ColorUI>();
    }

    // Use this for initialization
	void Start ()
    {
        shardsDisplay.text = ApplicationDatas.Game.Shards.ToString();
    }
	
    // return l'enemie le plus bas de la couleur
    // return null si pas de couleur
    private AKillable FindEntityByColor(int colorVal)
    {
        AKillable res = null;
        foreach(AKillable entity in entityList)
        {
            if (entity.GetColor() == colorVal)
            {
                if (res == null)
                    res = entity;
                else if (res != null && (res.transform.position.y > entity.transform.position.y))
                {
                    res = entity;
                }
            }
        }

        return res;
    }

    // Si ça hit, ça return true
    // Si il a tapé dans le vent, return false
    public AKillable HitEntity(int colorVal)
    {
        AKillable target;

        if ((target = FindEntityByColor(colorVal)) != null)
        {
            target.GetHit(1);
            return target;
        }
        else
            return null;
    }

    //Stock la valeur de la couleur dans l'argument. Nécessaire pour les ennemis qui changent de couleur après un coup
    public AKillable HitFirstEntity(ref int color)
    {
        AKillable res = null;
        foreach (AKillable entity in entityList)
        {
            if (entity.GetColor() != -1)
            {
                if (res == null)
                    res = entity;
                else if (res != null && (res.transform.position.y > entity.transform.position.y))
                {
                    res = entity;
                }
            }
        }

        if (res)
        {
            color = res.GetColor();
            res.GetHit(1);
        }
        return res;
    }

    //Touche tous les ennemis d'une certaine couleur
    public List<AKillable> HitAllEntities(int color)
    {
        List<AKillable> res = new List<AKillable>();

        foreach (AKillable entity in entityList)
        {
            if (entity.GetColor() == color)
            {
                int i = 0;
                while (i < res.Count && res[i].transform.position.y < entity.transform.position.y)
                {
                    ++i;
                }
                res.Insert(i, entity);
            }
        }
        foreach (AKillable entity in res)
        {
            entity.GetHit(1);
        }

        return (res.Count > 0 ? res : null);
    }

    //Touche tous les ennemis de la couleur du premier ennemi
    public List<AKillable> HitAllFirstEntities(ref int color)
    {
        List<AKillable> res = null;
        AKillable e = HitFirstEntity(ref color);

        if (e != null)
        {
            res = HitAllEntities(color);
            if (res == null)
            {
                res = new List<AKillable>();
            }
            res.Insert(0, e);
        }
        return res;
    }

    //ajoute un enemy dans la liste et lui attribue une couleur.
    public void AddEntity(AKillable obj)
    {
        entityList.Add(obj);
        //attribue la couleur d'affichage
    }

    //Retire l'ennemi de la liste
    public void RemoveEntity(AKillable obj)
    {
        entityList.Remove(obj);
    }

    public void ClearProjectiles()
    {
        List<AKillable> toRemove = new List<AKillable>();
        foreach (AKillable entity in entityList)
        {
            if (entity.GetComponent<AProjectile>() != null)
            {
                toRemove.Add(entity);
            }
        }
        foreach (AKillable entity in toRemove)
        {
            entityList.Remove(entity);
            GameObject.Destroy(entity.gameObject);
        }
        AProjectile[] leftovers = FindObjectsOfType<AProjectile>();
        for (int i = 0; i < leftovers.Length; ++i)
        {
            GameObject.Destroy(leftovers[i].gameObject);
        }
    }

    public void SetEntitiesSpeedModifier(float modifier)
    {
        foreach (AKillable e in entityList)
        {
            e.SetSpeedModifier(modifier);
        }
    }

    public Color GetColor(int index)
    {
        return availableColors[index];
    }

    public int GetUIColor(int index)
    {
        return uiColors[index];
    }

    public Color[] GetColors()
    {
        return availableColors;
    }

    public void ValidateHeroColor(int[] cols)
    {
        Color[] res = new Color[4];
        for (int i = 0; i < 4; ++i)
        {
            res[i] = GetColor(cols[i]);
            uiColors[i] = cols[i];
        }
        colorUI.PrintColor(res, uiColors);
    }

    public float GetPositionByColor(int color)
    {
        float ret = -3.0f;
        for (int i = 0; i < 4; ++i)
        {
            if (uiColors[i] == color)
            {
                return ret;
            }
            ret += 2.0f;
        }
        return 0.0f;
    }

    public void StartMultiColors()
    {
        Debug.Log(multicolor);
        foreach (Image button in buttons)
        {
            button.material = multicolor;
        }
    }

    public void StopMultiColors()
    {
        foreach (Image button in buttons)
        {
            button.material = null;
        }
    }

    public void GainShard()
    {
        shardsCount++;
        shardsDisplay.text = (ApplicationDatas.Game.Shards + shardsCount).ToString();
    }

    public void WinLevel()
    {
        levelEnded = true;
        victoryScreen.SetActive(true);
        victoryScreen.GetComponent<Animator>().SetTrigger("Win");
        if (ApplicationDatas.Game.CurrentLevel >= 0)
        {
            ApplicationDatas.Game.Shards += shardsCount;
            if (ApplicationDatas.Game.CurrentLevel + 1 > ApplicationDatas.Game.LevelsCleared && ApplicationDatas.Game.CurrentLevel < ApplicationDatas.MAX_LEVEL - 1)
            {
                ApplicationDatas.Game.LevelsCleared = ApplicationDatas.Game.CurrentLevel + 1;
            }
            ApplicationDatas.Save();
        }
    }

    public void LoseLevel()
    {
        levelEnded = true;
        gameOverScreen.SetActive(true);
        gameOverScreen.GetComponent<Animator>().SetTrigger("Die");
        if (ApplicationDatas.Game.CurrentLevel >= 0)
        {
            ApplicationDatas.Game.Shards += shardsCount / 3;
            ApplicationDatas.Save();
        }
    }

    public bool LevelEnded()
    {
        return levelEnded;
    }
}
