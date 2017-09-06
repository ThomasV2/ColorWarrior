using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HeroController : MonoBehaviour {

    [SerializeField]
    private int[] colors;
    [SerializeField]
    private int maxLife;
    [SerializeField]
    private GameObject attackPrefab;
    [SerializeField]
    private GameObject hammerPrefab;
    [SerializeField]
    private GameObject colorWavePrefab;
    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject bombPrefab;
    [SerializeField]
    private float invincibilityDuration;
    [SerializeField]
    private GameObject deathParticle;
    [SerializeField]
    private float deathDuration;
    [SerializeField]
    private AudioClip deathClip;
    [SerializeField]
    private List<AudioClip> shootClip;
    [SerializeField]
    private AudioClip hurtMistakeClip;
    [SerializeField]
    private AudioClip hurtHitClip;
    [SerializeField]
    private bool debug;

    private GameManager gManager;
    private BonusManager bManager;
    private InputsManager iManager;
    private InfosUI infoUI;
    private int life;
    private bool levelEnded = false;
    private Vector2 movement;
    private float invincibilityTimer;
    private bool shuffeling;
    private bool pause;

    //Death
    SpriteRenderer spriteRenderer;
    private bool dead = false;
    private float deathTimer;
    private Color dec;

    //Audio
    private AudioSource audioSource;
    private AudioManager audioManager;

	void Start ()
    {
        gManager = GameObject.FindObjectOfType<GameManager>();
        bManager = GameObject.FindObjectOfType<BonusManager>();
        iManager = GameObject.FindObjectOfType<InputsManager>();
        infoUI = GameObject.FindObjectOfType<InfosUI>();
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        colors = new int[4];
        ShuffleColors();
        maxLife += ApplicationDatas.Game.Bonus[Upgrades.LifeBonus];
        infoUI.InitLife(maxLife);
        life = maxLife;
        movement.y = speed;

        //On set les effets des bonus
        bManager.OnBonusGet(BonusName.LIFE, new BonusManager.BonusAction(WinLife));
        bManager.OnBonusGet(BonusName.MULTI_STRIKES, new BonusManager.BonusAction(() => gManager.StartMultiColors()));
        bManager.OnBonusLost(BonusName.MULTI_STRIKES, new BonusManager.BonusAction(() => gManager.StopMultiColors()));
	}
	
	void Update ()
    {
        invincibilityTimer -= (invincibilityTimer > 0 ? Time.deltaTime : 0);
        if (IsAlive())
        {
            if (!levelEnded)
            {
                Attack();
                if (Input.GetKeyDown(KeyCode.S))
                {
                    ShuffleColors();
                }
            }
            else
            {
                MoveToEnd();
            }
        }
        else
        {
            Death();
        }
    }

    private void Attack()
    {
        //Cannot attack during pause or while the colors are shuffeling
        if (pause || shuffeling) return;

        if (iManager.GetButtonDown("Fire1"))
        {
            FireTo(colors[0]);
        }
        if (iManager.GetButtonDown("Jump"))
        {
            FireTo(colors[1]);
        }
        if (iManager.GetButtonDown("Fire2"))
        {
            FireTo(colors[2]);
        }
        if (iManager.GetButtonDown("Fire3"))
        {
            FireTo(colors[3]);
        }
    }

    private void FireTo(int color)
    {
        if (bManager.IsActive(BonusName.DOOM_HAMMER))
        {
            List<AKillable> targets = (bManager.IsActive(BonusName.MULTI_STRIKES) ? gManager.HitAllFirstEntities(ref color) : gManager.HitAllEntities(color));

            if (targets == null)
            {
                GetHit(true);
            }
            else
            {
                audioSource.PlayOneShot(shootClip[Random.Range(0, shootClip.Count)], 1.0f);
                GameObject atk = Instantiate(hammerPrefab, this.transform.position, Quaternion.identity) as GameObject;
                atk.GetComponent<HammerAttack>().Init(targets, gManager.GetColor(color));
            }
        }
        else
        {
            //Si on est sous multiStrike, on choppe le premier ennemi peu importe la couleur pressée.
            AKillable target = (bManager.IsActive(BonusName.MULTI_STRIKES) ? gManager.HitFirstEntity(ref color) : gManager.HitEntity(color));

            if (target == null)
            {
                GetHit(true);
            }
            else
            {
                audioSource.PlayOneShot(shootClip[Random.Range(0, shootClip.Count)], 1.0f);
                GameObject atk = Instantiate(attackPrefab, this.transform.position, Quaternion.identity) as GameObject;
                atk.GetComponent<HeroAttack>().Init(target.transform, gManager.GetColor(color));
            }
        }
    }

    public bool IsAlive()
    {
        return (life > 0 || debug);
    }

    public void LoadColors()
    {

    }

    public int GetColor(int i)
    {
        return colors[i];
    }

    public void ShuffleColors(bool display = false)
    {
        int[] newKey = new int[4] {-1, -1, -1, -1};
        int i = 0;
        int newVal;
        bool isShuffleNeed;


        while (i < 4)
        {
            isShuffleNeed = false;
            newVal = Random.Range(0, gManager.GetColors().Length);
            if (gManager.GetColors().Length < 3)
            {
                Debug.LogError("need more color in GameManager");
                return;
            }

            for(int x = 0; x < 4; ++x)
            {
                if (newKey[x] == newVal)
                {
                    isShuffleNeed = true;
                    break;
                }
            }

            if (!isShuffleNeed)
            {
                newKey[i] = newVal;
                ++i;
            }
        }

        if (!display)
        {
            for (int x = 0; x < 4; ++x)
            {
                colors[x] = newKey[x];
            }
        }

        gManager.ValidateHeroColor(newKey);
    }

    public void ShiftColors(int shift)
    {
        int[] res = new int[colors.Length];
        shift %= colors.Length;

        for (int i = 0; i < colors.Length; ++i)
        {
            int pos = i - shift;
            if (pos < 0)
            {
                pos += colors.Length;
            }
            res[i] = colors[pos % colors.Length];
        }

        colors = res;
        gManager.ValidateHeroColor(colors);
    }

    public void GetHit(bool self = false)
    {
        if (invincibilityTimer <= 0)
        {
            life--;
            infoUI.LoseALife(life);
            if (life <= 0)
            {
                life = 0;
            }
            else
            {
                invincibilityTimer = invincibilityDuration;
                StartCoroutine(Blink());
                if (!self)
                {
                    audioSource.PlayOneShot(hurtHitClip);
                    StartCoroutine(ShuffleAfterHit());
                    GameObject obj = GameObject.Instantiate(bombPrefab, transform.position, transform.rotation) as GameObject;
                    obj.GetComponent<Bomb>().Init(false);
                }
                else
                {
                    audioSource.PlayOneShot(hurtMistakeClip);
                }
            }
        }
    }

    private IEnumerator Blink()
    {
        for (float i = 0; i < invincibilityDuration; i += 0.1f)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
        }
        spriteRenderer.enabled = true;
    }

    private IEnumerator ShuffleAfterHit()
    {
        shuffeling = true;
        ShuffleColors();
        for (float i = 0; i < invincibilityDuration; i += 0.05f)
        {
            ShuffleColors(true);
            yield return new WaitForSeconds(0.05f);
        }
        shuffeling = false;
        gManager.ValidateHeroColor(colors);
    }

    public void WinLife()
    {
        if (life < maxLife)
        {
            infoUI.WinALife(life++);
        }
    }

    public void LaunchColorWave(int colorLeft, int colorRight)
    {
        GameObject obj = Instantiate(colorWavePrefab, transform.position, transform.rotation) as GameObject;
        ColorWave cv = obj.GetComponent<ColorWave>();
        cv.Init(colorLeft, gManager.GetColor(colorLeft), colorRight, gManager.GetColor(colorRight));
    }

    public void EndLevel()
    {
        levelEnded = true;
        gManager.WinLevel();
        GetComponent<Animator>().SetFloat("Speed", 0.8f);
    }

    private void MoveToEnd()
    {
        if (transform.position.y < 7)
        {
            transform.Translate(movement * Time.deltaTime);
        }
    }

    private void Death()
    {
        if (!dead)
        {
            gManager.LoseLevel();
            audioSource.PlayOneShot(deathClip, 1.0f);
            audioManager.PlayDeath();
            dead = true;
            deathTimer = 0;
            FindObjectOfType<BackgroundGeneration>().Pause();
            GetComponent<Animator>().enabled = false;
            dec = spriteRenderer.color / deathDuration;
            dec.a = 0.0f;
        }
        if ((deathTimer += Time.deltaTime) < deathDuration)
        {
            spriteRenderer.color -= dec * Time.deltaTime;
        }
        else if (spriteRenderer.enabled)
        {
            spriteRenderer.enabled = false;
            GameObject d = Instantiate(deathParticle, transform.position, transform.rotation) as GameObject;
            GameObject.Destroy(d, d.GetComponent<ParticleSystem>().duration);
        }
    }

    public void Pause()
    {
        pause = true;
        Animator anim = GetComponent<Animator>();
        anim.ResetTrigger("Walk");
        anim.SetTrigger("Idle");
    }

    public void Resume()
    {
        pause = false;
        Animator anim = GetComponent<Animator>();
        anim.ResetTrigger("Idle");
        anim.SetTrigger("Walk");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Bonus bonus = other.GetComponent<Bonus>();
        if (bonus)
        {
            if (IsAlive())
            {
                bonus.GetEffect();
                infoUI.AddBonus((int)bonus.GetName(), bonus.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite, bonus.GetDuration(), bonus.GetComponent<SpriteRenderer>().color);
            }
            GameObject.Destroy(other.gameObject);
        }
    }
}
