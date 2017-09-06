using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BanditKing : ABoss
{
    public enum KBPhaseLimit
    {
        ATTACK = 3,
        RUN = 6,
        FINAL = 8
    };

    [SerializeField]
    private float sideStepSpeed;

    [SerializeField]
    private GameObject arrowPrefab;
    [SerializeField]
    private AudioClip shootArrowClip;
    [SerializeField]
    private float arrowDuration;
    [SerializeField]
    private float arrowCooldown;
    [SerializeField]
    private float arrowSpeed;

    [SerializeField]
    private GameObject daggerPrefab;
    [SerializeField]
    private AudioClip shootDaggerClip;
    [SerializeField]
    private float daggerDuration;
    [SerializeField]
    private float daggerCooldown;
    [SerializeField]
    private float daggerSpeed;

    [SerializeField]
    private GameObject bombPrefab;
    [SerializeField]
    private AudioClip throwBombClip;
    [SerializeField]
    private float bombDuration;
    [SerializeField]
    private float bombSpeed;
    [SerializeField]
    private float bombFuse;

    [SerializeField]
    private GameObject addsPrefab;
    [SerializeField]
    private float addsSpeedModifier;
    [SerializeField]
    private float addsSpawnCooldown;

    [SerializeField]
    private GameObject curseAOE;
    [SerializeField]
    private float AOESpeed;

    private Animator anim;

    private bool animationInit;
    private bool animationEnd;
    private Vector2 sideMovement;
    private Vector2 genericMovement;
    private float timer;
    private float attackTimer;
    private int currentPattern;
    private KBPhaseLimit currentPhase;
    private int lifeToPhase;
    private int lastAddsPos;
    private float rageMultiplier;
    private Color step;
    private List<BossAction> attackPatterns = new List<BossAction>();

    private Transform leftHand;
    private int leftHandColor;
    private Transform rightHand;
    private int rightHandColor;
    private bool hand;
    private SpriteRenderer bow;
    private SpriteRenderer bigCurse;

    private BonusManager bonusManager;
    private EnemySpawner enemySpawner;

    private string currentAnim;
    private int currentFinalColor;
    private int[] finalColors;

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
        sideMovement = new Vector2(sideStepSpeed, 0);
        leftHand = transform.Find("LeftHand");
        rightHand = transform.Find("RightHand");
        bonusManager = FindObjectOfType<BonusManager>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        bow = transform.Find("Bow").GetComponent<SpriteRenderer>();
        bigCurse = transform.Find("BigCurse").GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        currentAnim = "Walk";

        rageMultiplier = 1.0f;

        //AttackPhase
        attackPatterns.Add(new BossAction(FireArrows));
        attackPatterns.Add(new BossAction(Center));
        attackPatterns.Add(new BossAction(ThrowKnives));
        attackPatterns.Add(new BossAction(BombsAway));

        //RunPhase
        attackPatterns.Add(new BossAction(Center));
        attackPatterns.Add(new BossAction(RunAway));
        attackPatterns.Add(new BossAction(SpawnAdds));

        //FinalAttack
        attackPatterns.Add(new BossAction(FinalTransition));
        attackPatterns.Add(new BossAction(FinalAttack));

        actions[BossPhase.BEGIN] = new BossAction(Entrance);
        actions[BossPhase.PATTERN] = new BossAction(Patterns);
        actions[BossPhase.END] = new BossAction(Finish);

        hero.Pause();
        currentPhase = KBPhaseLimit.ATTACK;
    }

    public override void GetHit(int damage)
    {
        base.GetHit(damage);
        if ((currentLife <= life - 30 && lifeToPhase == 0)/* ||
            (currentLife <= life - 70 && lifeToPhase == 1)*/)
        {
            ++lifeToPhase;
            currentPattern = (int)KBPhaseLimit.ATTACK;
            currentPhase = KBPhaseLimit.RUN;
            timer = -1;
        }
        if (currentPhase == KBPhaseLimit.FINAL)
        {
            currentFinalColor += 1;
            currentFinalColor %= finalColors.Length;
            SetColor(finalColors[currentFinalColor]);
        }
    }

    public override void SetColor(int val)
    {
        base.SetColor(val);
        if (bigCurse != null)
        {
            Color c = gameManager.GetColor(color);
            bigCurse.color = c;
        }
    }

    public override void Kill(bool soft = true)
    {
        if (currentPattern != 1 && currentPattern <= (int)KBPhaseLimit.ATTACK)
        {
            timer = -1.0f;
        }
    }

    private void TriggerAnim(string name)
    {
        if (currentAnim != name)
        {
            anim.SetTrigger(name);
            currentAnim = name;
        }
    }

    private void ShowBow(bool state)
    {
        bow.enabled = state;
    }

    private void Entrance()
    {
        if (!animationInit)
        {
            GetComponent<BanditKingAnimation>().Play(BanditKingAnimation.Animations.ENTRANCE,
                new BanditKingAnimation.AnimationAction(() => {
                    InitArrow();
                    healthDisplay.gameObject.SetActive(true);
                    current = BossPhase.PATTERN;
                    hero.Resume();
                }));
            animationInit = true;
        }
    }

    private void Patterns()
    {
        if (timer <= 0)
        {
            ChooseNewPattern();
        }
        attackPatterns[currentPattern]();
        timer -= Time.deltaTime;
    }

    private void ChooseNewPattern()
    {
        ++currentPattern;
        if (currentPattern > (int)currentPhase)
        {
            currentPattern = 0;
        }
        attackTimer = 0.0f;
    }

    private void ThrowKnives()
    {
        if (timer <= 0)
        {
            ShowBow(false);
            timer = daggerDuration;
            hand = true;
            leftHandColor = Random.Range(0, 4);
            rightHandColor = (leftHandColor + 2) % 4;
            TriggerAnim("Throw");
            anim.SetFloat("Speed", rageMultiplier);
            attackTimer = daggerCooldown / rageMultiplier;
        }
        if ((attackTimer += Time.deltaTime) >= daggerCooldown / rageMultiplier)
        {
            attackTimer = 0;
            audioSource.PlayOneShot(shootDaggerClip);
            GameObject obj = GameObject.Instantiate(daggerPrefab, (hand ? rightHand.position : leftHand.position), daggerPrefab.transform.rotation) as GameObject;
            obj.GetComponent<Dagger>().Init((hand ? rightHand : leftHand), this, hero, daggerSpeed * rageMultiplier, (hand ? rightHandColor : leftHandColor));
            hand = !hand;
        }
    }

    private void InitArrow()
    {
        ShowBow(true);
        timer = arrowDuration;
        TriggerAnim("Walk");
        anim.SetFloat("Speed", rageMultiplier);
    }

    private void FireArrows()
    {
        if (timer <= 0)
        {
            InitArrow();
        }

        if ((transform.position.x > 3 && sideMovement.x > 0) || (transform.position.x < -3 && sideMovement.x < 0))
        {
            sideMovement.x *= -1;
        }
        transform.Translate(sideMovement * speedModifier * rageMultiplier * Time.deltaTime);

        if ((attackTimer += Time.deltaTime) >= arrowCooldown / rageMultiplier)
        {
            attackTimer = 0;
            audioSource.PlayOneShot(shootArrowClip);
            GameObject obj = GameObject.Instantiate(arrowPrefab, rightHand.position, arrowPrefab.transform.rotation) as GameObject;
            obj.GetComponent<AProjectile>().Init(this, hero, arrowSpeed * rageMultiplier);
        }
    }

    private void BombsAway()
    {
        if (timer <= 0)
        {
            ShowBow(false);
            TriggerAnim("Idle");
            float dir = (lifeToPhase > 0 ? 1 : 0);
            timer = bombDuration;
            audioSource.PlayOneShot(throwBombClip);
            for (int i = 0; i < lifeToPhase + 1; ++i)
            {
                GameObject obj = GameObject.Instantiate(bombPrefab, transform.position + new Vector3(0.0f, -0.2f, 0.0f), bombPrefab.transform.rotation) as GameObject;
                obj.GetComponent<SmallBomb>().Init(bombFuse / rageMultiplier, dir, this, hero, bombSpeed);
                dir *= -1;
            }
        }
    }

    private void Center()
    {
        if (timer <= 0)
        {
            ShowBow(false);
            TriggerAnim("Walk");
            anim.SetFloat("Speed", 2);
            float s = (sideStepSpeed * 2);
            timer = Mathf.Abs(transform.position.x) / s;
            genericMovement = new Vector2((transform.position.x < 0 ? s : -s), 0.0f);
        }
        transform.Translate(genericMovement * Time.deltaTime);
    }

    private void RunAway()
    {
        if (timer <= 0)
        {
            TriggerAnim("Rage");
            anim.SetFloat("Speed", 2);
            timer = 4.0f;
            genericMovement = new Vector2(0.0f, 0.0f);
        }
        else if (genericMovement.y == 0 && timer < 2.0f)
        {
            TriggerAnim("Walk");
            float s = 10 / timer;
            genericMovement = new Vector2(0.0f, s);
        }
        transform.Translate(genericMovement * Time.deltaTime);
    }

    private void SpawnAdds()
    {
        if (timer <= 0)
        {
            TriggerAnim("Idle");
            transform.position = new Vector3(0.0f, transform.position.y, 0.0f);
            genericMovement = new Vector2(0.0f, -FindObjectOfType<BackgroundGeneration>().GetSpeed());
        }
        timer = 100.0f;
        transform.Translate(genericMovement * Time.deltaTime);
        if (transform.position.y > 6.0f && (attackTimer += Time.deltaTime) >= addsSpawnCooldown / rageMultiplier)
        {
            attackTimer = 0.0f;
            int posX;
            do
            {
                posX = Random.Range(-3, 4);
            } while (posX == lastAddsPos);
            lastAddsPos = posX;
            int col = Random.Range(0, 4);
            for (int i = 0; i < 2; ++i)
            {
                GameObject obj = GameObject.Instantiate(addsPrefab, new Vector2(posX, 6.0f + (i * 1f)), addsPrefab.transform.rotation) as GameObject;
                obj.GetComponent<AEnemy>().Init(hero, enemySpawner.GetLevelSpeed() * addsSpeedModifier * rageMultiplier, enemySpawner.GetSpeedModifier(), bonusManager.GetRandomBonus(), col);
            }
        }
        if (transform.position.y <= 2.8f)
        {
            transform.position = new Vector3(0.0f, 2.8f, 0.0f);
            timer = -1;
            rageMultiplier += 0.3f;
            if (lifeToPhase <= 1)
            {
                currentPhase = KBPhaseLimit.ATTACK;
            }
            else
            {
                currentPhase = KBPhaseLimit.FINAL;
            }
        }
    }

    private void FinalTransition()
    {
        if (timer <= 0.0f)
        {
            hero.Pause();
            TriggerAnim("Idle");
            step = (spriteRenderer.color - new Color(0.1f, 0.1f, 0.1f)) / 3.0f;
            finalColors = new int[2];
            finalColors[0] = Random.Range(0, 4);
            finalColors[1] = (finalColors[0] + 1) % 4;
            SetColor(finalColors[currentFinalColor]);
        }
        timer = 100.0f;
        if ((attackTimer += Time.deltaTime) < 4.0f)
        {
            if (attackTimer < 3.0f)
            {
                spriteRenderer.color -= step * Time.deltaTime;
            }
            else
            {
                bigCurse.enabled = true;
                bigCurse.color = new Color(bigCurse.color.r, bigCurse.color.g, bigCurse.color.b, 1.0f - (4.0f - attackTimer));
            }
        }
        else
        {
            bigCurse.color = new Color(bigCurse.color.r, bigCurse.color.g, bigCurse.color.b);
            hero.Resume();
            timer = -1;
        }
    }

    private void FinalAttack()
    {
        if (timer <= 0.0f)
        {
            GameObject obj = GameObject.Instantiate(curseAOE);
            obj.transform.parent = this.transform;
            obj.GetComponent<CurseAOE>().Init(AOESpeed);
        }
        timer = 100.0f;
    }

    private void Finish()
    {
        if (!animationEnd)
        {
            animationEnd = true;
            healthDisplay.gameObject.SetActive(false);
            GetComponent<BanditKingAnimation>().Play(BanditKingAnimation.Animations.DEATH,
            new BanditKingAnimation.AnimationAction(() => {
                hero.Resume();
                hero.EndLevel();
                GameObject.Destroy(this.gameObject);
            }));
        }
    }
}
