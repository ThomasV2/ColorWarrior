using UnityEngine;
using System.Collections;

public class HeroAttack : MonoBehaviour {
    [SerializeField]
    private ParticleSystem partColor;
    [SerializeField]
    private SpriteRenderer baseColor;
    [SerializeField]
    private int speed;
    [SerializeField]
    private float range;

    private Transform target;

    public void Init(Transform obj, Color col)
    {
        target = obj;
        baseColor.color = col;
        partColor.startColor = col;
    }

    void Update()
    {
        if (target != null)
        {
            if (Vector2.Distance(this.transform.position, target.position) < range)
            {
                AKillable ak = target.GetComponent<AKillable>();
                ak.PlayHit();
                ak.isHitByAttack = true;

                //On détruit le particle system 0.3f secondes plus tard pour lui laisser le temps de finir l'animation
                partColor.loop = false;
                partColor.transform.parent = null;
                GameObject.Destroy(partColor.gameObject, 0.3f);

                GameObject.Destroy(this.gameObject);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            }
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
