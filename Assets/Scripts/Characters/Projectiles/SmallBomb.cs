using UnityEngine;
using System.Collections;

public class SmallBomb : AProjectile
{
    [SerializeField]
    private GameObject bombSpread;
    [SerializeField]
    private float spreadSpeed;

    private bool armed;
    private bool bounce;
    private bool explode;
    private Color colored;
    private Color step;

    private float fuse;
    private float fuseTimer;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        armed = true;
        bounce = false;
        fuseTimer = 0.0f;
    }

    //Direction should be between -1 and 1
    public void Init(float time, float direction, AKillable own, HeroController target, float s, int c = -1)
    {
        base.Init(own, target, s, c);
        fuse = time;
        spriteRenderer = GetComponent<SpriteRenderer>();
        colored = gameManager.GetColor(color);
        step = (colored - new Color(0.3f, 0.3f, 0.3f)) / fuse;
        spriteRenderer.color = colored;
        movement = new Vector2(direction * speed, -speed);
    }

    // No sound when hit
    public override void PlayHit() {}

    // Update is called once per frame
    void Update()
    {
        if (armed)
        {
            if ((fuseTimer += Time.deltaTime) > fuse)
            {
                Explode(true);
            }
            else if (fuseTimer < fuse / 2)
            {
                transform.Translate(movement * (1.0f - (fuseTimer / (fuse / 2))) * Time.deltaTime);
            }
            spriteRenderer.color -= step * Time.deltaTime;
        }
        else if (bounce)
        {
            transform.position = Vector2.MoveTowards(transform.position, owner.transform.position, 6.0f * Time.deltaTime);
        }
    }

    public override void GetHit(int damage = 1)
    {
        isHitByAttack = false;
        armed = false;
        StartCoroutine(WaitForHit());
    }

    public override void Kill(bool soft = true)
    {
        armed = false;
        GameObject.Destroy(this.gameObject);
    }

    private IEnumerator WaitForHit()
    {
        gameManager.RemoveEntity(this);
        while (isHitByAttack == false)
        {
            yield return null;
        }
        bounce = true;
    }

    private void Explode(bool spread = false)
    {
        explode = true;
        gameManager.RemoveEntity(this);
        GameObject particle = transform.Find("Smoke").gameObject;
        particle.SetActive(true);
        GetComponent<AudioSource>().Play();
        GameObject.Destroy(this.gameObject, 0.5f);
        spriteRenderer.enabled = false;
        this.enabled = false;

        if (spread)
        {
            particle.GetComponent<ParticleSystem>().startColor = Color.black;
            SpawnSpread(transform.position);
            /*SpawnSpread(new Vector2(transform.position.x, transform.position.y - 0.3f));
            SpawnSpread(new Vector2(transform.position.x - 0.3f, transform.position.y + 0.1f));
            SpawnSpread(new Vector2(transform.position.x + 0.3f, transform.position.y + 0.1f));*/
        }
        else
        {
            particle.GetComponent<ParticleSystem>().startColor = colored;
        }
    }

    private void SpawnSpread(Vector2 position)
    {
        GameObject obj = GameObject.Instantiate(bombSpread, position, bombSpread.transform.rotation) as GameObject;
        obj.GetComponent<Throwable>().Init(this, hero, spreadSpeed, color);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!explode && bounce && other.transform == owner.transform)
        {
            owner.GetHit(15);
            Explode();
        }
    }
}
