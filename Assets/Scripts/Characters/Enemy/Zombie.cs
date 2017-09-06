using UnityEngine;
using System.Collections;

public class Zombie : AEnemy {

    [SerializeField]
    private GameObject grave;

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
        GameObject obj = Instantiate(grave, transform.position, transform.rotation) as GameObject;
        obj.transform.parent = FindObjectOfType<EnemySpawner>().transform;

        Grave g = obj.GetComponent<Grave>();
        g.Init(gameManager, hero, color);

        GameObject.Destroy(this.gameObject);
    }
}
