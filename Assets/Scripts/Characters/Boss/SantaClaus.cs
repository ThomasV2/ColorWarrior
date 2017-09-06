using UnityEngine;
using System.Collections;


public class SantaClaus : ABoss {

    [SerializeField]
    private GameObject _giftPref;
    [SerializeField]
    private int _nrbDamageTakable;
    [SerializeField]
    private float _atkTimerMax;
    [SerializeField]
    private float _atkColldownMax;
    [SerializeField]
    private float _laughtTimerMax;
    [SerializeField]
    private GameObject _curse;

    private BossAction _attack;
    private BossAction _laught;
    private bool animationInit;
    private bool animationEnd;
    private float _atkTimer;
    private float _atkColldown;
    private float _laughtTime;
    private int rageMultiplier;
    private int damageTake;

    [SerializeField]
    private Transform leftHand;
    [SerializeField]
    private Transform rightHand;
    private bool hand;

    private bool _isLaughting;
    [SerializeField]
    private SpriteRenderer _curseColor;

    [SerializeField]
    private Animator animSanta;
    //Entrance
    [SerializeField]
    private float entranceSpeed;
    private Vector2 movement;
    //exit
    private int ExitPhase;
    private bool isShakeLeft;

    protected override void Start()
    {
        base.Start();
        ExitPhase = -1;
        movement = new Vector2(0, -entranceSpeed);
        actions[BossPhase.BEGIN] = new BossAction(Entrance);
        actions[BossPhase.PATTERN] = new BossAction(Patterns);
        actions[BossPhase.END] = new BossAction(Finish);
        rageMultiplier = 1;
        hero.Pause();
    }

    private void Entrance()
    {
        if (!animationInit)
        {
            if (transform.position.y > 2.8f)
            {
                transform.Translate(movement * Time.deltaTime);
            }
            else
            {
                animationInit = true;
                healthDisplay.gameObject.SetActive(true);
                _atkTimer = _atkTimerMax;
                _atkColldown = 0f;
                _isLaughting = false;
                current = BossPhase.PATTERN;
                animSanta.SetBool("Atk", true);
                hero.Resume();
            }
        }
    }

    private void Patterns()
    {
        if (_atkTimer <= 0)
        {
            if (_isLaughting == false)
            {
                LaughtInit();
                _isLaughting = true;
            }
            else
                Laught();
        }
        else
            Attack();
        _atkTimer -= Time.deltaTime;
    }

    public void FinishExit()
    {
        ExitPhase++;
        if (ExitPhase == 1)
            animSanta.SetBool("Walk", true);
    }

    private void Finish()
    {
        if (ExitPhase == -1)
        {
            if (isShakeLeft == true)
            {
                if (transform.position.x > 0.05f)
                {
                    isShakeLeft = !isShakeLeft;
                }
                else
                {
                    Vector2 move = new Vector2(entranceSpeed, 0);
                    this.transform.Translate(move * Time.deltaTime);
                }
            }
            else
            {
                if (transform.position.x < -0.05f)
                {
                    isShakeLeft = !isShakeLeft;
                }
                else
                {
                    Vector2 move = new Vector2(-entranceSpeed, 0);
                    this.transform.Translate(move * Time.deltaTime);
                }
            }
        }
        else if (ExitPhase == 1)
        {
            if (transform.position.x < 12)
            {
                Vector2 move = new Vector2 (speed * 2.5f, 0);
                this.transform.Translate(move * Time.deltaTime);
            }
            else
            {
                hero.Resume();
                hero.EndLevel();
                GameObject.Destroy(this.gameObject);
            }
        }

    }

    private void Attack()
    {
        if ((_atkColldown -= Time.deltaTime) <= 0)
        {
            _atkColldown = _atkColldownMax;
            //audioSource.PlayOneShot();
            GameObject obj = Instantiate(_giftPref, (hand ? rightHand.position : leftHand.position), Quaternion.identity) as GameObject;
            obj.GetComponent<Gift>().Init(this, hero, 2 * rageMultiplier);
            hand = !hand;
        }
    }

    private void LaughtInit()
    {
        animSanta.SetTrigger("Laught");
        SetColor(-1);
        Color c = gameManager.GetColor(color);
        _curseColor.color = c;
        _isLaughting = true;
        _laughtTime = _laughtTimerMax;
    }

    private void Laught()
    {
        damageTake = 0;
        _laughtTime -= Time.deltaTime;
        if (_laughtTime <=0 || damageTake >= _nrbDamageTakable)
        {
            RemoveColor();
            _curseColor.color = Color.black;
            _atkTimer = _atkTimerMax;
            _atkColldown = 0.5f;
            _isLaughting = false;
            animSanta.SetTrigger("EndLaught");
        }
    }

    public void SwapColors()
    {
        int tmpCol = color;
        while (color == tmpCol)
        {
            SetColor(-1);
        }
        Color c = gameManager.GetColor(color);
        _curseColor.color = c;
    }

    public override void GetHit(int damage)
    {
        base.GetHit(20);
        RemoveColor();
        _curseColor.color = Color.black;
        //change de couleur
        if (currentLife <= 0)
        {
            //plop
            animSanta.SetBool("Atk", false);
            animSanta.SetBool("IsDead", true);
        }
        else
        {
            damageTake++;
            Invoke("SwapColors", 0.5f);
        }
    }

    public override void Kill(bool soft = true)
    {
    }

}
