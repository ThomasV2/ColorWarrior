using UnityEngine;
using System.Collections;

public class MagicBandit : AEnemy
{
    private int nextColor;
    private bool move;
    private float destination;
    [SerializeField]
    private SpriteRenderer panel;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        transform.Translate(movement * speedModifier * Time.deltaTime);
        if ((!fleeing || !isHitByAttack) && !attacking && move)
        {
            Vector2 dest = new Vector2(destination, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, dest, 7 * speedModifier * Time.deltaTime);
            if (transform.position.x == destination)
            {
                move = false;
            }
        }
    }

    public override float SetMovement(float speedY, float speedX = 0)
    {
        float speedTot = base.SetMovement(speedY, speedX);
        panel.transform.GetComponent<Animator>().SetFloat("Speed", speedTot);
        return speedTot;
    }

    public override void SetColor(int val)
    {
        base.SetColor(val);
        do
        {
            nextColor = Random.Range(0, 4);
        } while (nextColor == color);
        panel.color = gameManager.GetColor(nextColor);
    }

    public override void GetHit(int damage = 1)
    {
        base.GetHit(damage);

        if (!fleeing)
        {
            SetColor(nextColor);
            move = true;
            destination = gameManager.GetPositionByColor(color);
        }
        if (life <= 1)
        {
            panel.enabled = false;
        }
    }
}
