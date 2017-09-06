using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grave : AKillable {

    [SerializeField]
    private List<Sprite> sprites;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Material shakingMaterial;
    [SerializeField]
    private GameObject zombiePrefab;
    [SerializeField]
    private int resProb;
    [SerializeField]
    private float resDuration;

    private int colorValue;
    private Color step;
    private Vector2 movement;
    private bool resurrection = false;
    private float resTimer;
    bool active = true;

    private GameManager gManager;
    private EnemySpawner eSpawner;

	// Use this for initialization
	void Start ()
    {
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Count)];
        movement.y = -FindObjectOfType<BackgroundGeneration>().GetSpeed();
        eSpawner = FindObjectOfType<EnemySpawner>();
	}

    public void Init(GameManager gm, HeroController h, int c)
    {
        base.Init(h, 1.0f);
        gManager = gm;
        colorValue = c;
        RemoveColor();
    }

	// Update is called once per frame
	void Update ()
    {
        if (active)
        {
            if (!gManager.LevelEnded())
            {
                transform.Translate(movement * Time.deltaTime);
            }
            if (!resurrection && transform.position.y >= hero.transform.position.y + 3f && Random.Range(0, 1000) <= resProb)
            {
                resurrection = true;
                SetColor(colorValue);
                Color color = gManager.GetColor(colorValue);
                spriteRenderer.material = shakingMaterial;
                resTimer = 0;
                step = (spriteRenderer.color - color) / resDuration;
            }
            if (resurrection)
            {
                Ressurect();
            }
        }
        if (transform.position.y < -5.5)
        {
            GameObject.Destroy(this.gameObject);
        }
	}

    private IEnumerator WaitForHit()
    {
        gameManager.RemoveEntity(this);
        while (isHitByAttack == false)
        {
            yield return null;
        }
        PlayHit();
        GetComponent<SpriteRenderer>().enabled = false;
        GameObject.Destroy(this.gameObject, audioSource.clip.length);
    }

    public override void GetHit(int damage = 1)
    {
        isHitByAttack = false;
        active = false;
        StartCoroutine(WaitForHit());
    }

    public override void Kill(bool soft)
    {
        GameObject.Destroy(this.gameObject);
    }

    private void Ressurect()
    {
        if ((resTimer += Time.deltaTime) > resDuration)
        {
            GameObject obj = GameObject.Instantiate(zombiePrefab, transform.position, transform.rotation) as GameObject;
            obj.transform.parent = eSpawner.transform;
            AEnemy zombie = obj.GetComponent<AEnemy>();
            zombie.Init(hero, eSpawner.GetLevelSpeed(), 1.0f, null, colorValue);

            GameObject.Destroy(this.gameObject);
        }
        else
        {
            spriteRenderer.color -= step * Time.deltaTime;
            spriteRenderer.material.SetColor("_Color", spriteRenderer.color);
        }
    }
}
