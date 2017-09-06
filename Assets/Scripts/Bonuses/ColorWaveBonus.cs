using UnityEngine;
using System.Collections;

public class ColorWaveBonus : Bonus {

    private int colorLeft;
    private int colorRight;

	// Use this for initialization
	void Start ()
    {
        colorLeft = Random.Range(0, 4);
        do
        {
            colorRight = Random.Range(0, 4);
        } while (colorLeft == colorRight);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        GameManager gm = FindObjectOfType<GameManager>();
        sr.material.SetColor("_ColorLeft", gm.GetColor(colorLeft));
        sr.material.SetColor("_ColorRight", gm.GetColor((int)colorRight));
    }

    public override void GetEffect()
    {
        hero.GetComponent<HeroController>().LaunchColorWave(colorLeft, colorRight);
    }
}
