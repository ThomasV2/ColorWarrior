using UnityEngine;
using System.Collections;

public class ShadowGuard : AEnemy {

    [SerializeField]
    private GameObject smokeParticle;
    [SerializeField]
    private float showDist;
    private bool visible;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        RemoveColor();
        visible = false;
        curse.enabled = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        transform.Translate(movement * speedModifier * Time.deltaTime);
        if (!visible && transform.position.y < hero.transform.position.y + showDist)
        {
            visible = true;
            curse.enabled = true;
            SetColor(-1);
        }
    }

    protected override void EndAction()
    {
        GameObject smoke = Instantiate(smokeParticle, transform.position, transform.rotation) as GameObject;
        GameObject.Destroy(smoke, smoke.GetComponent<ParticleSystem>().duration);
        GameObject.Destroy(this.gameObject);
    }
}
