using UnityEngine;
using System.Collections;
using System;

public abstract class AProjectile : AKillable
{
    protected AKillable owner;

    public void Init(AKillable own, HeroController target, float s, int color = -1)
    {
        base.Init(target, 1.0f, color);
        owner = own;
        speed = s;
    }

    protected void LookAt(Transform target)
    {
        Vector2 diff = target.position - transform.position;
        diff.Normalize();
        LookAt(diff);
    }

    protected void LookAt(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);
    }
}
