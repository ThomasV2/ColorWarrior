using UnityEngine;
using System.Collections;

public class ColorWave : MonoBehaviour {

    [SerializeField]
    private float speed;

    private int colorLeft;
    private int colorRight;
	
    public void Init(int valLeft, Color cl, int valRight, Color cr)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        colorLeft = valLeft;
        colorRight = valRight;
        sr.material.SetColor("_ColorLeft", cl);
        sr.material.SetColor("_ColorRight", cr);
    }

	// Update is called once per frame
	void Update ()
    {
        float inc = speed * Time.deltaTime;
        transform.localScale = new Vector3(transform.localScale.x + inc, transform.localScale.y + inc, transform.localScale.z + inc);
	    if (transform.localScale.x >= 30)
        {
            GameObject.Destroy(this.gameObject);
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        AEnemy enemy = other.GetComponent<AEnemy>();
        if (enemy)
        {
            if (enemy.transform.position.x >= 0)
            {
                enemy.SetColor(colorRight);
            }
            else
            {
                enemy.SetColor(colorLeft);
            }
        }
    }
}
