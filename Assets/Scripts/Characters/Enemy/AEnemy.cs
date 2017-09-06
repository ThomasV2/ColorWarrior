using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public abstract class AEnemy : AKillable
{
    //Tableaux contenant les différents assets d'habillage
    [SerializeField]
    private RuntimeAnimatorController[] characters;
    [SerializeField]
    private Sprite[] hairs;
    [SerializeField]
    private Sprite[] heads;
    [SerializeField]
    private Sprite[] torsos;
    [SerializeField]
    private RuntimeAnimatorController[] legs;
    [SerializeField]
    private Sprite[] weapons;
    [SerializeField]
    private Sprite[] shields;

    [SerializeField]
    protected SpriteRenderer curse;

    //SpriteRenderer à update lors des changements de position
    [SerializeField]
    private SpriteRenderer[] renderers;
    private List<int> defaultOrderValues;

    //Quantité de drop à la mort + prefab
    [SerializeField]
    private uint shards;
    [SerializeField]
    private GameObject shardPrefab;

    //Spellrenderer de l'enemy
    [SerializeField]
    private SpriteRenderer spell;
    [SerializeField]
    private SpriteRenderer spellBG;

    //Slow effect game object
    [SerializeField]
    private SpriteRenderer slow;
    private Vector3 slowRotationSpeed;

    [SerializeField]
    protected int life = 1;
    protected Vector2 movement;
    protected GameObject bonus;
    protected bool fleeing = false;
    protected bool attacking = false;
    protected bool casting = false;

    private float castTimer = 0.0f;
    [SerializeField]
    private float castTime;

    [SerializeField]
    private int runningSpeed = 6;

    // Use this for initialization
    protected virtual void Start()
    {
        if (characters.Length > 0)
        {
            Animator a = transform.GetComponent<Animator>();
            a.runtimeAnimatorController = characters[Random.Range(0, characters.Length)];
            //Quickfix so the animator component doesn't freeze
            a.enabled = false;
            a.enabled = true;
        }
        if (hairs.Length > 0)
        {
            transform.Find("Hair").GetComponent<SpriteRenderer>().sprite = hairs[Random.Range(0, hairs.Length)];
        }
        if (heads.Length > 0)
        {
            transform.Find("Head").GetComponent<SpriteRenderer>().sprite = heads[Random.Range(0, heads.Length)];
        }
        if (torsos.Length > 0)
        {
            transform.Find("Torso").GetComponent<SpriteRenderer>().sprite = torsos[Random.Range(0, torsos.Length)];
        }
        if (legs.Length > 0)
        {
            transform.Find("Legs").GetComponent<Animator>().runtimeAnimatorController = legs[Random.Range(0, legs.Length)];
        }
        else
        {
            transform.Find("Legs").GetComponent<Animator>().enabled = false;
        }
        if (weapons.Length > 0)
        {
            transform.Find("Weapon").GetComponent<SpriteRenderer>().sprite = weapons[Random.Range(0, weapons.Length)];
        }
        if (shields.Length > 0)
        {
            transform.Find("Shield").GetComponent<SpriteRenderer>().sprite = shields[Random.Range(0, shields.Length)];
        }
        slowRotationSpeed = new Vector3(0, 0, 360);

        //Ajoute les valeurs par défaut de l'ordre de tri des SpriteRenderer
        defaultOrderValues = new List<int>();
        foreach (SpriteRenderer render in renderers)
        {
            defaultOrderValues.Add(render.sortingOrder);
        }
    }

    public virtual void Init(HeroController hero, float s, float sModifier, GameObject b, int color = -1)
    {
        base.Init(hero, sModifier, color);
        SetMovement(s * speed);
        bonus = b;
    }

    // Update is called once per frame
    protected virtual void Update() {
        //Si l'ennemi sort de l'écran, on le détruit
        if (transform.position.x > hero.transform.position.x + 10 ||
            transform.position.x < hero.transform.position.x - 10 ||
            transform.position.y < hero.transform.position.y - 5)
        {
            GameObject.Destroy(this.gameObject);
        }
        //Si l'ennemi est proche du hero, il l'attaque.
        if (transform.position.y < hero.transform.position.y + 2 && hero.IsAlive())
        {
            Attack();
        }
        //Si l'ennemi dépasse la position du héro, il n'est plus tuable
        if (transform.position.y <= hero.transform.position.y)
        {
            gameManager.RemoveEntity(this);
        }

        //On affiche ou non l'effet de slow en fonction de speedModifier
        if (speedModifier < 1 && slow.enabled == false)
        {
            slow.enabled = true;
        }
        else if (speedModifier >= 1 && slow.enabled)
        {
            slow.enabled = false;
        }
        if (slow.enabled)
        {
            slow.transform.Rotate(slowRotationSpeed * Time.deltaTime);
        }

        //On change l'ordre d'affichage des renderers en fonction de la position de l'ennemi
        for (int i = 0; i < renderers.Length; ++i)
        {
            renderers[i].sortingOrder = (int)(-transform.position.y * 100) + defaultOrderValues[i];
        }
        Casting();
    }

    private void SpawnBonus()
    {
        if (bonus)
        {
            GameObject obj = Instantiate(bonus, transform.position, transform.rotation) as GameObject;
            Bonus b = obj.GetComponent<Bonus>();
            b.Init(hero.transform);
        }
    }

    private float GetFastSpeed()
    {
        float tot = Mathf.Abs(movement.x) + Mathf.Abs(movement.y);
        return (tot > runningSpeed ? tot : runningSpeed);
    }

    public virtual float SetMovement(float speedY, float speedX = 0)
    {
        movement = new Vector2(speedX, -speedY);
        float speedTot = (Mathf.Abs(speedY) + Mathf.Abs(speedX)) * 0.2f;
        GetComponent<Animator>().SetFloat("Speed", speedTot);
        transform.Find("Legs").GetComponent<Animator>().SetFloat("Speed", speedTot);
        return speedTot;
    }

    public override void SetSpeedModifier(float modifier)
    {
        if (!fleeing)
        {
            base.SetSpeedModifier(modifier);
        }
    }

    public override void GetHit(int damage = 1)
    {
        if (!fleeing)
        {
            if (--life <= 0)
            {
                isHitByAttack = false;
                StartCoroutine(Freedom());
            }
        }
    }

    public override void SetColor(int val)
    {
        base.SetColor(val);
        Color c = gameManager.GetColor(color);
        curse.color = c;
        spellBG.color = c;
        spellBG.material.SetColor("_Color", c);
    }

    public override void RemoveColor()
    {
        base.RemoveColor();
        Color c = new Color();
        curse.color = c;
        spellBG.color = c;
        spellBG.material.SetColor("_Color", c);
    }

    //Quand l'ennemi est suffisemment proche du joueur, il lui court dessus.
    protected virtual void Attack()
    {
        if (!fleeing && !attacking)
        {
            attacking = true;
            Vector2 heading = hero.transform.position - transform.position;
            float tot = Mathf.Abs(heading.x) + Mathf.Abs(heading.y);
            if (tot > 0)
            {
                heading.x /= tot;
                heading.y /= tot;
                float sp = GetFastSpeed();
                SetMovement(sp * -heading.y, sp * heading.x);
            }
        }
    }

    //Lance le sort de l'ennemi
    protected virtual void CastSpell()
    {
        castTimer = 0;
        casting = true;
        GetComponent<Animator>().SetBool("Casting", true);
        transform.Find("Legs").GetComponent<Animator>().SetBool("Casting", true);

        spell.enabled = true;
        spellBG.enabled = true;
    }

    //Called during the casting
    private void Casting()
    {
        if (casting)
        {
            if ((castTimer += Time.deltaTime) >= castTime)
            {
                SpellEffect();
                EndCast();
            }
            else
            {
                spellBG.material.SetFloat("_Opacity", castTimer / castTime);
            }
        }
    }

    protected virtual void SpellEffect() { }

    //Cast cleanup
    private void EndCast()
    {
        castTimer = 0;
        spell.enabled = false;
        spellBG.enabled = false;
        casting = false;

        GetComponent<Animator>().SetBool("Casting", false);
        transform.Find("Legs").GetComponent<Animator>().SetBool("Casting", false);
    }

    private void DropColorShards()
    {
        for (uint i = 0; i < shards; ++i)
        {
            Vector3 pos = curse.transform.position;
            pos.x += Random.Range(-0.1f, 0.1f);
            pos.y += Random.Range(-0.1f, 0.1f);
            GameObject obj = Instantiate(shardPrefab, pos, transform.rotation) as GameObject;
            ColorShards cs = obj.GetComponent<ColorShards>();
            cs.Init(i, curse.color, curse.transform, gameManager);
        }
    }

    //Appelé quand l'ennemi est "tué" par le joueur. L'enlève du gamemanager et le fait s'enfuir en X.
    //On retire également sa malédiction
    private IEnumerator Freedom()
    {
        fleeing = true;
        gameManager.RemoveEntity(this);

        while (isHitByAttack == false)
        {
            yield return null;
        }

        EndCast();
        SpawnBonus();
        DropColorShards();
        EndAction();
    }

    public override void Kill(bool soft = true)
    {
        gameManager.RemoveEntity(this);
        fleeing = true;

        EndCast();
        if (soft)
        {
            SpawnBonus();
            DropColorShards();
        }
        EndAction();
    }

    protected virtual void EndAction()
    {
        float sp = GetFastSpeed();
        SetMovement(0, (transform.position.x > hero.transform.position.x ? sp : -sp));

        transform.Find("Curse").GetComponent<SpriteRenderer>().enabled = false;
        transform.Find("Background").GetComponent<SpriteRenderer>().enabled = false;

        speedModifier = 1.0f;
        slow.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!fleeing && other.transform == hero.transform)
        {
            hero.GetHit();
            gameManager.RemoveEntity(this);
        }
    }
}
