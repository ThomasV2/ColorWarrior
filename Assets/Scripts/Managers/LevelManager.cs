using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [System.Serializable]
    class LevelInfos
    {
        public float duration = 0;
        public GameObject boss = null;
    }

    [SerializeField]
    private HeroController hero;
    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private BackgroundGeneration bgGeneration;

    [SerializeField]
    private int level;
    [SerializeField]
    private List<LevelInfos> infos;
    [SerializeField]
    private bool debug;

    private float timerDuration = 0;

    private bool endLevel = false;
    private bool waitForPlayer = false;

	// Use this for initialization
	void Start ()
    {
        // Si ApplicationDatas.Level vaut -1, on ne vient pas de l'overworld donc on est en debug
        if (ApplicationDatas.Game.CurrentLevel >= 0)
        {
            level = ApplicationDatas.Game.CurrentLevel;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (debug || !hero.IsAlive() || waitForPlayer) return;

        if (!endLevel || enemySpawner.transform.childCount == 0)
        {
            timerDuration += Time.deltaTime;
        }
        if (timerDuration>= infos[level].duration && !endLevel)
        {
            endLevel = true;
            enemySpawner.Pause();
        }
        if (endLevel && timerDuration > infos[level].duration + 2.0f && enemySpawner.transform.childCount == 0)
        {
            waitForPlayer = true;
            bgGeneration.Pause();
            if (infos[level].boss != null)
            {
                GameObject obj = GameObject.Instantiate(infos[level].boss);
                ABoss boss = obj.GetComponent<ABoss>();
                boss.Init(hero, 1.0f);
                boss.RemoveColor();
            }
            else
            {
                hero.EndLevel();
            }
        }
    }
}
