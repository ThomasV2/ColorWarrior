using UnityEngine;
using System.Collections;

public class CurseAOE : MonoBehaviour
{
    private Color step;
    private SpriteRenderer spriteRenderer;

	public void Init(float growth)
    {
        step = new Color(0, 0, 0, growth);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (spriteRenderer.color.a < 1.0f)
        {
            spriteRenderer.color += step * Time.deltaTime;
        }
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (spriteRenderer.color.a >= 1.0f)
        {
            HeroController hc = other.GetComponent<HeroController>();
            if (hc != null)
            {
                hc.GetHit();
            }
        }
    }
}
