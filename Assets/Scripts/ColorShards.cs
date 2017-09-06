using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorShards : MonoBehaviour {

    [SerializeField]
    private float speed;
    [SerializeField]
    private float idleSpeed;
    [SerializeField]
    private float baseDelay;
    [SerializeField]
    private float indexDelay;

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private List<Sprite> shards;

    private float delay;
    private float delayTimer;
    private Vector3 target;
    private Vector3 direction;
    private GameManager gameManager;

	// Use this for initialization
	public void Init (uint index, Color c, Transform cPos, GameManager gm)
    {
        delay = baseDelay + index * indexDelay;
        delayTimer = 0;
        target = new Vector3(-8.3f, 4.5f, 0.0f);
        spriteRenderer.sortingOrder = (int)index;
        spriteRenderer.color = c;
        spriteRenderer.sprite = shards[Random.Range(0, shards.Count)];
        direction = cPos.position - transform.position;
        direction = direction.normalized;
        gameManager = gm;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if ((delayTimer += Time.deltaTime) > delay)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            if (Vector2.Distance(this.transform.position, target) < 0.2f)
            {
                gameManager.GainShard();
                GameObject.Destroy(this.gameObject);
            }
        }
        else
        {
            transform.Translate(-direction * idleSpeed * ((delay - delayTimer) / delay) * Time.deltaTime);
        }
	}
}
