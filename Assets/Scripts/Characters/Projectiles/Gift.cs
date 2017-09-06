﻿using UnityEngine;
using System.Collections;

public class Gift : AProjectile {

    private bool active;
    private Transform hand;

    void Start()
    {
        active = true;
    }

    void Update()
    {
        Vector2 direction = new Vector2(0, -speed);
        transform.Translate(direction * Time.deltaTime, Space.World);
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
