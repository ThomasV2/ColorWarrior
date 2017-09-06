using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DarkTeaBoltAnimation : MonoBehaviour
{

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
    private Dictionary<Animations, List<AnimationAction>> animations = new Dictionary<Animations, List<AnimationAction>>();

    [SerializeField]
    private AudioClip bossMusic;

    //Entrance
    [SerializeField]
    private float entranceSpeed;
    private Vector2 movement;

    // Use this for initialization
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        play = Animations.NONE;
        animator = GetComponent<Animator>();
        movement = new Vector2(0, -entranceSpeed);

        animations.Add(Animations.ENTRANCE, new List<AnimationAction>());

        animations[Animations.ENTRANCE].Add(new AnimationAction(MoveDown));
        animations[Animations.ENTRANCE].Add(new AnimationAction(End));

        animations.Add(Animations.DEATH, new List<AnimationAction>());

        animations[Animations.DEATH].Add(new AnimationAction(RunAway));
        animations[Animations.DEATH].Add(new AnimationAction(End));
    }

    // Update is called once per frame
    void Update()
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
        if (transform.position.y > 3.5f)
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
