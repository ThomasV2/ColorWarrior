using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BanditKingAnimation : MonoBehaviour {

    public enum Animations
    {
        NONE,
        ENTRANCE,
        DEATH
    };
    public delegate void AnimationAction();

    private AudioManager audioManager;

    private Animations play;
    private int idx;
    private float timer;
    private bool init;
    private float waitTime;
    private Animator animator;
    private AnimationAction callback;
    private Dictionary<Animations, List<AnimationAction> > animations = new Dictionary<Animations, List<AnimationAction> >();

    [SerializeField]
    private AudioClip bossMusic;

    //Entrance
    [SerializeField]
    private float entranceSpeed;
    private Vector2 movement;

    [SerializeField]
    private Transform curse;

    //CurseShake
    [SerializeField]
    private float curseShakeTimer;

    //CurseMove
    [SerializeField]
    private float curseTimer;

    [SerializeField]
    private float growTimer;
    [SerializeField]
    private float growSize;
    private float defaultSize;
    private Vector3 growStep;

	// Use this for initialization
	void Start ()
    {
        audioManager = FindObjectOfType<AudioManager>();
        play = Animations.NONE;
        animator = GetComponent<Animator>();
        movement = new Vector2(0, -entranceSpeed);
        growStep = Vector3.one * ((growSize - transform.localScale.x) / growTimer);
        defaultSize = transform.localScale.x;

        animations.Add(Animations.ENTRANCE, new List<AnimationAction>());

        animations[Animations.ENTRANCE].Add(new AnimationAction(MoveDown));
        animations[Animations.ENTRANCE].Add(new AnimationAction(Wait));
        animations[Animations.ENTRANCE].Add(new AnimationAction(ShakeCurse));
        animations[Animations.ENTRANCE].Add(new AnimationAction(CurseDown));
        animations[Animations.ENTRANCE].Add(new AnimationAction(Wait));
        animations[Animations.ENTRANCE].Add(new AnimationAction(Grow));
        animations[Animations.ENTRANCE].Add(new AnimationAction(Wait));
        animations[Animations.ENTRANCE].Add(new AnimationAction(End));

        animations.Add(Animations.DEATH, new List<AnimationAction>());

        animations[Animations.DEATH].Add(new AnimationAction(Shrink));
        animations[Animations.DEATH].Add(new AnimationAction(Wait));
        animations[Animations.DEATH].Add(new AnimationAction(RunAway));
        animations[Animations.DEATH].Add(new AnimationAction(End));
    }

    // Update is called once per frame
    void Update ()
    {
	    if (play != Animations.NONE)
        {
            animations[play][idx]();
        }
	}

    public void Play(Animations anim, AnimationAction cb)
    {
        play = anim;
        idx = 0;
        callback = cb;
    }

    private void Next()
    {
        timer = 0;
        init = false;
        idx++;
    }

    private void MoveDown()
    {
        if (!init)
        {
            init = true;
            audioManager.FadeOut(2.0f);
        }
        if (transform.position.y > 2.8f)
        {
            transform.Translate(movement * Time.deltaTime);
        }
        else
        {
            animator.SetTrigger("Idle");
            waitTime = 2;
            Next();
        }
    }

    private void Wait()
    {
        if ((timer += Time.deltaTime) > waitTime)
        {
            Next();
        }
    }

    private void ShakeCurse()
    {
        if (!init)
        {
            curse.GetComponent<Animator>().SetTrigger("Shake");
            init = true;
        }

        if ((timer += Time.deltaTime) > curseShakeTimer)
        {
            curse.GetComponent<Animator>().SetTrigger("Shake");
            Next();
        }
    }

    private void CurseDown()
    {
        if (!init)
        {
            curse.GetComponent<Animator>().SetTrigger("Down");
            init = true;
        }
        if ((timer += Time.deltaTime) > curseTimer)
        {
            waitTime = 0.5f;
            curse.gameObject.SetActive(false);
            Next();
        }
    }

    private void Grow()
    {
        if (!init)
        {
            init = true;
            audioManager.PlayClip(bossMusic);
            audioManager.FadeIn(1.0f);
            animator.SetTrigger("Rage");
        }
        transform.localScale += growStep * Time.deltaTime;
        if ((timer += Time.deltaTime) > curseTimer)
        {
            animator.SetTrigger("Walk");
            FindObjectOfType<BackgroundGeneration>().Resume();
            waitTime = 1.0f;
            transform.localScale = Vector3.one * growSize;
            Next();
        }
    }

    private void Shrink()
    {
        if (!init)
        {
            init = true;
            audioManager.FadeOut(1.0f);
            animator.SetTrigger("Stunned");
        }
        transform.localScale -= growStep * Time.deltaTime;
        if ((timer += Time.deltaTime) > curseTimer)
        {
            waitTime = 2.0f;
            transform.localScale = Vector3.one * defaultSize;
            Next();
        }
    }

    private void RunAway()
    {
        if (!init)
        {
            init = true;
            movement = new Vector2(0, 3);
            animator.SetTrigger("Walk");
            audioManager.PlayBack();
            audioManager.FadeIn(1.0f);
        }
        transform.Translate(movement * Time.deltaTime);
        if (transform.position.y > 6)
        {
            Next();
        }
    }

    private void End()
    {
        play = Animations.NONE;
        if (callback != null)
        {
            callback();
        }
    }
}
