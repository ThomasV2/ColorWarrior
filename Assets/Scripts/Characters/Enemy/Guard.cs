using UnityEngine;
using System.Collections;

public class Guard : AEnemy
{

    private bool hitted;
    private float beatTimer;
    private EnemySpawner enemySpawner;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        transform.Translate((hitted && beatTimer < enemySpawner.GetBeatTime() / 1.5f ? -2 : 1) * movement * speedModifier * Time.deltaTime);
        beatTimer += Time.deltaTime;
    }

    protected override void EndAction()
    {
        base.EndAction();
        hitted = false;
    }

    public override void GetHit(int damage = 1)
    {
        base.GetHit(damage);

        if (!fleeing && !attacking)
        {
            hitted = true;
            beatTimer = 0;
        }
    }
}
