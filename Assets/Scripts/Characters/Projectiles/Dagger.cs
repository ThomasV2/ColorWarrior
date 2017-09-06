using UnityEngine;
using System.Collections;

public class Dagger : AProjectile
{
    private bool active;
    private Transform hand;

    void Start()
    {
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Mathf.Abs(hand.position.y - hero.transform.position.y);
        float travel = Mathf.Abs(transform.position.y - hand.position.y);
        Vector2 direction = new Vector2(speed, -speed);
        direction.x *= Mathf.Abs(travel - distance / 2) / (distance / 2);
        direction.x *= (travel < distance / 2 ? 1 : -1);
        direction.x *= (transform.position.x < 0 ? -1 : 1);
        LookAt(direction);
        transform.Translate(direction * Time.deltaTime, Space.World);
    }

    public void Init(Transform h, AKillable own, HeroController target, float s, int c = -1)
    {
        base.Init(own, target, s, c);
        hand = h;
    }

    public override void SetColor(int val)
    {
        base.SetColor(val);
        Color c = gameManager.GetColor(color);
        GetComponent<SpriteRenderer>().color = c;
    }

    public override void GetHit(int damage = 1)
    {
        isHitByAttack = false;
        active = false;
        StartCoroutine(WaitForHit());
    }

    public override void Kill(bool soft = true)
    {
        active = false;
        gameManager.RemoveEntity(this);
        GameObject.Destroy(this.gameObject);
    }

    private IEnumerator WaitForHit()
    {
        gameManager.RemoveEntity(this);
        while (isHitByAttack == false)
        {
            yield return null;
        }
        Kill(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (active && other.transform == hero.transform)
        {
            hero.GetHit();
            GameObject.Destroy(this.gameObject);
            gameManager.RemoveEntity(this);
        }
    }
}
