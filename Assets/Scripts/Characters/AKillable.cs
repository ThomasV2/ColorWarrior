using UnityEngine;
using System.Collections;

public abstract class AKillable : MonoBehaviour
{
    protected int color;
    [SerializeField]
    protected float speed;
    protected float speedModifier = 1.0f;

    protected GameManager gameManager;
    protected HeroController hero;
    protected AudioSource audioSource;

    public bool isHitByAttack;

    public void Init(HeroController h, float sModifier, int color = -1)
    {
        gameManager = FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();
        hero = h;
        speedModifier = sModifier;
        SetColor(color);
        gameManager.AddEntity(this);
    }

    protected virtual void OnDestroy()
    {
        gameManager.RemoveEntity(this);
    }

    public virtual void SetSpeedModifier(float modifier)
    {
        speedModifier = modifier;
    }

    public virtual void PlayHit()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    public abstract void GetHit(int damage);

    public abstract void Kill(bool soft);

    public int GetColor()
    {
        return color;
    }

    public virtual void SetColor(int val)
    {
        if (val == -1)
            color = Random.Range(0, 4);
        else
            color = val;
    }

    public virtual void RemoveColor()
    {
        color = -1;
    }

    //A adapter si le gameManager set la couleur
    public void ChangeColor()
    {
        int newColor = Random.Range(0, 4);
        while (newColor == color)
        {
            newColor = Random.Range(0, 4);
        }
        SetColor(newColor);
    }
}
