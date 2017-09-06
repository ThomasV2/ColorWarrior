using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HammerAttack : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem partColor;
    [SerializeField]
    private SpriteRenderer baseColor;
    [SerializeField]
    private int speed;
    [SerializeField]
    private float range;

    private List<AKillable> targets;
    private Vector3 rotation;

    private void Start()
    {
        rotation = new Vector3(0, 0, 720);
    }

    public void Init(List<AKillable> t, Color col)
    {
        targets = t;
        baseColor.color = col;
        partColor.startColor = col;
    }

    void Update()
    {
        this.transform.Rotate(rotation * Time.deltaTime);
        if (targets != null)
        {
            if (Vector2.Distance(this.transform.position, targets[0].transform.position) < range)
            {
                targets[0].PlayHit();
                targets[0].isHitByAttack = true;

                targets.RemoveAt(0);
                if (targets.Count == 0)
                {
                    partColor.loop = false;
                    partColor.transform.parent = null;

                    Vector3 scale = partColor.transform.localScale;
                    scale.x /= this.transform.localScale.x;
                    scale.y /= this.transform.localScale.z;
                    scale.z /= this.transform.localScale.x;
                    partColor.transform.localScale = scale;

                    //On détruit le particle system 0.3f secondes plus tard pour lui laisser le temps de finir l'animation
                    GameObject.Destroy(partColor.gameObject, 0.3f);
                    GameObject.Destroy(this.gameObject);
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, targets[0].transform.position, speed * Time.deltaTime);
            }
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
