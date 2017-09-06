using UnityEngine;
using System.Collections;

public enum BonusName
{
    LIFE,
    MULTI_STRIKES,
    DOOM_HAMMER,
    SLOW,
    COLOR_WAVE
};

public class Bonus : MonoBehaviour {

    const float SCALE_DISTANCE = 2;

    [SerializeField]
    private BonusName type;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float duration;

    private Vector2 movement;
    private Vector2 defaultScale;

    protected Transform hero;

    // Use this for initialization
	public void Init(Transform h)
    {
        defaultScale = transform.localScale;
        hero = h;
        Vector2 heading = hero.position - transform.position;
        float tot = Mathf.Abs(heading.x) + Mathf.Abs(heading.y);
        if (tot > 0)
        {
            heading.x /= tot;
            heading.y /= tot;
            movement = new Vector2(speed * heading.x, speed * heading.y);
        }
    }
	
	// Update is called once per frame
	protected void Update ()
    {
        transform.Translate(movement * Time.deltaTime);

        Vector2 dist = hero.position - transform.position;
        float tot = Mathf.Abs(dist.x) + Mathf.Abs(dist.y);
        if (tot < SCALE_DISTANCE)
        {
            transform.localScale = tot / SCALE_DISTANCE * defaultScale;
        }
    }

    public float GetDuration()
    {
        return duration;
    }

    public BonusName GetName()
    {
        return type;
    }

    public virtual void GetEffect()
    {
        GameObject.FindObjectOfType<BonusManager>().ApplyBonus(type, duration);
    }
}
