using UnityEngine;
using System.Collections;

public class Shadow : AEnemy {

    [SerializeField]
    private GameObject smokeParticle;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        transform.Translate(movement * speedModifier * Time.deltaTime);
    }

    protected override void EndAction()
    {
        GameObject smoke = Instantiate(smokeParticle, transform.position, transform.rotation) as GameObject;
        GameObject.Destroy(smoke, smoke.GetComponent<ParticleSystem>().duration);
        GameObject.Destroy(this.gameObject);
    }
}
