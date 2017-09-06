using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum BossPhase
{
    BEGIN,
    PATTERN,
    END
};

public abstract class ABoss : AKillable
{
    [SerializeField]
    protected int life;
    protected Slider healthDisplay;
    protected int currentLife;

    public delegate void BossAction();

    protected BossPhase current;
    protected Dictionary<BossPhase, BossAction> actions = new Dictionary<BossPhase, BossAction>();

    protected SpriteRenderer spriteRenderer;
    private float blinkDuration = 1.0f;
    private float blinkTimer;

    protected virtual void Start()
    {
        healthDisplay = GameObject.Find("Canvas/Infos/BossHealth").GetComponent<Slider>();
        currentLife = life;
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthDisplay.value = GetPercentLife();
    }

    void Update()
    {
        actions[current]();
    }

    public override void GetHit(int damage)
    {
        currentLife -= damage;
        healthDisplay.value = GetPercentLife();
        if (currentLife <= 0)
        {
            current = BossPhase.END;
            hero.Pause();
            FindObjectOfType<BackgroundGeneration>().Pause();
            gameManager.ClearProjectiles();
        }
        else
        {
            Blink();
        }
    }

    public int GetLife()
    {
        return currentLife;
    }

    //Retourn le pourcentage de vie restante entre 0 et 1
    public float GetPercentLife()
    {
        float clampLife = (currentLife < 0 ? 0 : currentLife);
        return clampLife / life;
    }

    protected void Blink()
    {
        bool start = (blinkTimer <= 0);
        blinkTimer = blinkDuration;
        if (start)
        {
            StartCoroutine(BlinkRoutine());
        }
    }

    private IEnumerator BlinkRoutine()
    {
        while (blinkTimer > 0)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            blinkTimer -= 0.1f;
        }
        spriteRenderer.enabled = true;
    }
}
