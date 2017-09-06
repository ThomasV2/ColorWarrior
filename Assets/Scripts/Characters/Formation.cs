using UnityEngine;
using System.Collections;

public class Formation : MonoBehaviour {

    [SerializeField]
    private GameObject entity;

    public GameObject GetEntity()
    {
        return entity;
    }
}
