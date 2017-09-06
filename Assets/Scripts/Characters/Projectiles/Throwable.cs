using UnityEngine;
using System.Collections;

public class Throwable : AProjectile
{
    private bool active;

    void Start()
    {
        active = true;
    }

    public override void SetColor(int val)
    {
        base.SetColor(val);
        Color c = gameManager.GetColor(color);
        GetComponent<SpriteRenderer>().color = c;
    }

    // Update is called once per frame
    void Update ()
    {
        Vector3 diff = hero.transform.position - transform.position;
        diff.Normalize();
        float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);

        transform.position = Vector2.MoveTowards(transform.position, hero.transform.position, speed * Time.deltaTime);
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
