using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    class LevelInfos
    {
        public float speed = 0;
        public float beatTime = 0;
        public List<BeatFormation> formations = new List<BeatFormation>();
    }

    private GameManager gameManager;
    private BonusManager bonusManager;

    //Transform du player. Nécessaire pour les déplacements de certains ennemis.
    [SerializeField]
    private HeroController hero;

    [SerializeField]
    private float spawnHeight;
    [SerializeField]
    private List<LevelInfos> levels;
    [SerializeField]
    private int level = 0;

    //timer des formations
    private float beatTimer;
    private BeatFormation currentFormation;
    private bool halfBeat;

    //speed modifier for slow bonus
    private float speedModifier;

    [SerializeField]
    private bool paused = false;

    private List<int> defaultColors;
    private int[] constRandomColors;

    void Start ()
    {
        // Si ApplicationDatas.Level vaut -1, on ne vient pas de l'overworld donc on est en debug
        if (ApplicationDatas.Game.CurrentLevel >= 0)
        {
            level = ApplicationDatas.Game.CurrentLevel;
            if (level >= levels.Count)
            {
                level = 0;
            }
        }
        gameManager = GameObject.FindObjectOfType<GameManager>();
        bonusManager = GameObject.FindObjectOfType<BonusManager>();
        beatTimer = 0.0f;
        speedModifier = 1.0f;

        defaultColors = new List<int>();
        defaultColors.Add(0);
        defaultColors.Add(1);
        defaultColors.Add(2);
        defaultColors.Add(3);
        constRandomColors = new int[4];

        bonusManager.OnBonusGet(BonusName.SLOW, new BonusManager.BonusAction(() => { speedModifier = 0.5f; gameManager.SetEntitiesSpeedModifier(speedModifier); }));
        bonusManager.OnBonusLost(BonusName.SLOW, new BonusManager.BonusAction(() => { speedModifier = 1.0f; gameManager.SetEntitiesSpeedModifier(speedModifier); }));
        UpdateCurrentBeatFormation();
    }

    void Update()
    {
        if (paused || levels[level].formations.Count == 0) return;

        if (level < levels.Count)
        {
            List<BeatFormation.FormationInfos> formations = null;

            beatTimer += Time.deltaTime;
            if (!halfBeat && beatTimer >= levels[level].beatTime / 2 / speedModifier && (formations = currentFormation.GetHalfBeat()) != null)
            {
                halfBeat = true;
                for (int i = 0; i < formations.Count; ++i)
                {
                    SpawnEntity(formations[i].formation, formations[i].color);
                }
                UpdateCurrentBeatFormation();
            }
            if (beatTimer >= levels[level].beatTime / speedModifier)
            {
                halfBeat = false;
                beatTimer %= (levels[level].beatTime / speedModifier);
                if ((formations = currentFormation.GetBeat()) != null)
                {
                    for (int i = 0; i < formations.Count; ++i)
                    {
                        SpawnEntity(formations[i].formation, formations[i].color);
                    }
                    UpdateCurrentBeatFormation();
                }
            }
        }
    }

    private void SpawnEntity(Formation formation, ColorPicker color)
    {
        GameObject spawn = formation.GetEntity();

        Vector3 start = formation.transform.position;
        start.y = spawnHeight;

        if (color >= ColorPicker.LEFT && color <= ColorPicker.RIGHT)
        {
            switch (color)
            {
                case ColorPicker.LEFT:
                    start.x = -3;
                    break;
                case ColorPicker.MLEFT:
                    start.x = -1;
                    break;
                case ColorPicker.MRIGHT:
                    start.x = 1;
                    break;
                case ColorPicker.RIGHT:
                    start.x = 3;
                    break;
            }
        }

        GameObject obj = Instantiate(spawn, start, spawn.transform.rotation) as GameObject;
        obj.transform.parent = this.transform;
        AEnemy enemy = obj.GetComponent<AEnemy>();
        enemy.Init(hero, levels[level].speed, speedModifier, bonusManager.GetRandomBonus(), ColorPickerToColor(color));
    }

    private void UpdateCurrentBeatFormation()
    {
        if (levels[level].formations.Count == 0) return;

        if (currentFormation == null || currentFormation.End())
        {
            List<int> tmpColors = new List<int>(defaultColors);
            for (int i = 0; i < 4; ++i)
            {
                int idx = Random.Range(0, tmpColors.Count);
                constRandomColors[i] = tmpColors[idx];
                tmpColors.RemoveAt(idx);
            }
            currentFormation = levels[level].formations[Random.Range(0, levels[level].formations.Count)];
            currentFormation.Init();
        }
    }

    public float GetLevelSpeed()
    {
        return levels[level].speed;
    }

    public float GetSpeedModifier()
    {
        return speedModifier;
    }

    public void Pause()
    {
        paused = true;
    }

    public void Resume()
    {
        paused = false;
    }

    public int ColorPickerToColor(ColorPicker picker)
    {
        switch (picker)
        {
            //using hero.GetColor instead of gamemanager.GetUiColors because we have to get the final color when shuffeling
            case ColorPicker.LEFT:
                return hero.GetColor(0);
            case ColorPicker.MLEFT:
                return hero.GetColor(1);
            case ColorPicker.MRIGHT:
                return hero.GetColor(2);
            case ColorPicker.RIGHT:
                return hero.GetColor(3);
            case ColorPicker.CONST_RAND_1:
                return constRandomColors[0];
            case ColorPicker.CONST_RAND_2:
                return constRandomColors[1];
            case ColorPicker.CONST_RAND_3:
                return constRandomColors[2];
            case ColorPicker.CONST_RAND_4:
                return constRandomColors[3];
        }
        return -1;
    }

    public float GetBeatTime()
    {
        return levels[level].beatTime;
    }
}
