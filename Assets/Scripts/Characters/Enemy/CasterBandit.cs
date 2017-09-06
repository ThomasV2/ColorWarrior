using UnityEngine;
using System.Collections;

public class CasterBandit : AEnemy {

    private int castCount = 0;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (!casting)
        {
            transform.Translate(movement * speedModifier * Time.deltaTime);
        }
        if (castCount == 0 && Mathf.Abs(hero.transform.position.y - transform.position.y) < 4)
        {
            castCount++;
            CastSpell();
        }
    }

    protected override void SpellEffect()
    {
        hero.ShiftColors(1);
    }
}
