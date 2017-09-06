using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameDatas
{
    public int CurrentLevel { get; set; }
    public int LevelsCleared { get; set; }
    public int Shards { get; set; }

    public Dictionary<Upgrades, int> Bonus { get; set; }

    public GameDatas()
    {
        LevelsCleared = 0;
        CurrentLevel = -1;
        Shards = 0;

        Bonus = new Dictionary<Upgrades, int>();
        foreach (Upgrades val in System.Enum.GetValues(typeof(Upgrades)))
        {
            Bonus.Add(val, 0);
        }
    }
}
