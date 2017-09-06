using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

    [SerializeField]
    private float speed;
    private bool hardKill;

    public void Init(bool hk)
    {
        hardKill = hk;
    }

    // Update is called once per frame
    void Update()
    {
        float inc = speed * Time.deltaTime;
        transform.localScale = new Vector3(transform.localScale.x + inc, transform.localScale.y + inc, transform.localScale.z + inc);
        if (transform.localScale.x >= 30)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        AKillable entity = other.GetComponent<AKillable>();
        if (entity)
        {
            entity.Kill(hardKill);
        }
    }
}
