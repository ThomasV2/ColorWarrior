using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpdateLayerOrder : MonoBehaviour {

    private List<SpriteRenderer> renderers;

	// Use this for initialization
	void Start ()
    {
        renderers = new List<SpriteRenderer>();
        renderers.Add(this.GetComponent<SpriteRenderer>());
        foreach (Transform child in transform)
        {
            renderers.Add(child.GetComponent<SpriteRenderer>());
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        foreach (SpriteRenderer rend in renderers)
        {
            rend.sortingOrder = (int)(-transform.position.y * 100);
        }
    }
}
