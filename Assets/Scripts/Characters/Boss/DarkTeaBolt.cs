using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DarkTeaBolt : ABoss
{
    bool animationInit = false;
    bool animationEnd = false;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        actions[BossPhase.BEGIN] = new BossAction(Entrance);
        actions[BossPhase.PATTERN] = new BossAction(Patterns);
        actions[BossPhase.END] = new BossAction(Finish);

        hero.Pause();
    }

    public override void GetHit(int damage)
    {
        base.GetHit(damage);
    }

    public override void SetColor(int val)
    {
        base.SetColor(val);
    }

    public override void Kill(bool soft = true)
    {
    }

    private void Entrance()
    {
        if (!animationInit)
        {
            GetComponent<DarkTeaBoltAnimation>().Play(DarkTeaBoltAnimation.Animations.ENTRANCE,
                new DarkTeaBoltAnimation.AnimationAction(() => {
                    healthDisplay.gameObject.SetActive(true);
                    current = BossPhase.PATTERN;
                    hero.Resume();
                }));
            animationInit = true;
        }
    }

    private void Patterns()
    {
    }

    private void Finish()
    {
        if (!animationEnd)
        {
            animationEnd = true;
            healthDisplay.gameObject.SetActive(false);
            GetComponent<DarkTeaBoltAnimation>().Play(DarkTeaBoltAnimation.Animations.DEATH,
            new DarkTeaBoltAnimation.AnimationAction(() => {
                hero.Resume();
                hero.EndLevel();
                GameObject.Destroy(this.gameObject);
            }));
        }
    }
}
