using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum Upgrades
{
    Empty,
    LifeBonus,
    LifeDrop
};

public class Shop : MonoBehaviour
{
    [System.Serializable]
    class UpgradeCost
    {
        [System.Serializable]
        class NameCostTuple
        {
            public Upgrades bonus = 0;
            public List<int> costs = new List<int>();
        }

        [SerializeField]
        private List<NameCostTuple> bonusCosts;

        public List<int> this[Upgrades key]
        {
            get
            {
                return GetValue(key);
            }
        }

        private List<int> GetValue(Upgrades key)
        {
            foreach (NameCostTuple tuple in bonusCosts)
            {
                if (tuple.bonus == key)
                {
                    return tuple.costs;
                }
            }
            return null;
        }
    }

    [SerializeField]
    private Text description;
    [SerializeField]
    private Text title;
    [SerializeField]
    private Sprite bonusBought;
    [SerializeField]
    private UpgradeCost prices;
    [SerializeField]
    private Text shards;
    [SerializeField]
    private AudioClip purchase;
    [SerializeField]
    private AudioClip purchaseError;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        foreach (Transform upgrade in transform.Find("Upgrades"))
        {
            UpdateUpgradeLevel(upgrade);
            DisableUnusedLevel(upgrade);
        }
        UpdateShards();
    }

    public void UpdateDescription(Text newDescription)
    {
        description.text = newDescription.text;
    }

    public void UpdateTitle(Text newTitle)
    {
        title.text = newTitle.text;
    }

    public void BuyBonus(GameObject node)
    {
        Upgrades bonus = (Upgrades)System.Enum.Parse(typeof(Upgrades), node.name);
        if (ApplicationDatas.Game.Bonus[bonus] < prices[bonus].Count &&
            ApplicationDatas.Game.Shards >= prices[bonus][ApplicationDatas.Game.Bonus[bonus]])
        {
            audioSource.clip = purchase;
            ApplicationDatas.Game.Shards -= prices[bonus][ApplicationDatas.Game.Bonus[bonus]];
            ApplicationDatas.Game.Bonus[bonus]++;
            UpdateUpgradeLevel(node.transform);
        }
        else
        {
            audioSource.clip = purchaseError;
        }
        audioSource.Play();
    }

    private void UpdateUpgradeLevel(Transform upgrade)
    {
        Upgrades bonus = (Upgrades)System.Enum.Parse(typeof(Upgrades), upgrade.name);
        Transform level = upgrade.Find("Group/Level");
        for (int i = 0; i < ApplicationDatas.Game.Bonus[bonus]; ++i)
        {
            level.GetChild(i).GetComponent<Image>().sprite = bonusBought;
        }
        Text price = upgrade.Find("Group/Price/Text").GetComponent<Text>();
        if (ApplicationDatas.Game.Bonus[bonus] < prices[bonus].Count)
        {
            price.text = prices[bonus][ApplicationDatas.Game.Bonus[bonus]].ToString();
        }
        else
        {
            upgrade.Find("Group/Price/MAX").gameObject.SetActive(true);
            price.gameObject.SetActive(false);
            upgrade.Find("Group/Price/Image").gameObject.SetActive(false);
        }
        UpdateShards();
        ApplicationDatas.Save();
    }

    private void DisableUnusedLevel(Transform upgrade)
    {
        Upgrades bonus = (Upgrades)System.Enum.Parse(typeof(Upgrades), upgrade.name);
        Transform level = upgrade.Find("Group/Level");
        for (int i = prices[bonus].Count; i < level.childCount; ++i)
        {
            level.GetChild(i).GetComponent<Image>().enabled = false;
        }
    }

    private void UpdateShards()
    {
        shards.text = ApplicationDatas.Game.Shards.ToString();
    }
}
