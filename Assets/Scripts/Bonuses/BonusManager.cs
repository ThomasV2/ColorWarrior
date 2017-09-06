using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BonusManager : MonoBehaviour
{
    [System.Serializable]
    class BonusInfos
    {
        public GameObject bonus = null;
        // % de spawn sur 100 (méthode temporaire en attendant la boutique)
        public float probability = 0.0f;
    }

    [SerializeField]
    private List<BonusInfos> bonuses;


    class BonusTimer
    {
        public float timer = 0;
        public float duration = 0;
    }

    public delegate void BonusAction();

    Dictionary<BonusName, BonusTimer> timers = new Dictionary<BonusName, BonusTimer>();
    Dictionary<BonusName, BonusAction> onBonusOn = new Dictionary<BonusName, BonusAction>();
    Dictionary<BonusName, BonusAction> onBonusOff = new Dictionary<BonusName, BonusAction>();

    void Start()
    {
        if (bonuses.Count > 0)
        {
            bonuses[0].probability = 0.3f * ApplicationDatas.Game.Bonus[Upgrades.LifeDrop];
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (KeyValuePair<BonusName, BonusTimer> timer in timers)
        {
            if (timer.Value.duration > 0.0f)
            {
                if ((timer.Value.timer += Time.deltaTime) > timer.Value.duration)
                {
                    timer.Value.duration = 0.0f;
                    if (onBonusOff.ContainsKey(timer.Key))
                    {
                        onBonusOff[timer.Key]();
                    }
                }
            }
        }
    }

    public void ApplyBonus(BonusName name, float duration)
    {
        if (!timers.ContainsKey(name))
        {
            timers[name] = new BonusTimer();
        }

        timers[name].timer = 0;
        if (timers[name].duration == 0.0f && onBonusOn.ContainsKey(name))
        {
            onBonusOn[name]();
        }
        timers[name].duration = duration;
    }

    public void OnBonusGet(BonusName name, BonusAction action)
    {
        if (onBonusOn.ContainsKey(name))
        {
            onBonusOn[name] += action;
        }
        else
        {
            onBonusOn[name] = action;
        }
    }

    public void OnBonusLost(BonusName name, BonusAction action)
    {
        if (onBonusOff.ContainsKey(name))
        {
            onBonusOff[name] += action;
        }
        else
        {
            onBonusOff[name] = action;
        }
    }

    public bool IsActive(BonusName name)
    {
        return (timers.ContainsKey(name) && timers[name].duration > 0.0f);
    }


    public GameObject GetRandomBonus()
    {
        foreach (BonusInfos info in bonuses)
        {
            if (Random.Range(0.0f, 100.0f) <= info.probability)
            {
                return info.bonus;
            }
        }
        return null;
    }
}
