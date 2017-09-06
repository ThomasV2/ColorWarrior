using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class OWHeroController : MonoBehaviour {

    [SerializeField]
    private float speed;
    [SerializeField]
    private List<GameObject> levels;
    [SerializeField]
    private int levelCleared; //For debug
    [SerializeField]
    private Text levelDisplay;
    [SerializeField]
    private AudioClip levelSelect;
    [SerializeField]
    private AudioClip enterLevelClip;

    private Animator animator;
    private AudioSource audioSource;

    private bool movement;
    private int position;
    private GameObject destination;

    private Vector3 offset;
    private bool active;

    private InputsManager inputsManager;

	// Use this for initialization
	void Start ()
    {
        levelCleared = (levelCleared < 0 ? ApplicationDatas.Game.LevelsCleared : levelCleared);
        active = true;
        inputsManager = FindObjectOfType<InputsManager>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        position = (ApplicationDatas.Game.CurrentLevel >= 0 ? ApplicationDatas.Game.CurrentLevel : 0);
        offset.y = GetComponent<SpriteRenderer>().sprite.bounds.size.y * transform.localScale.y / 2;
        transform.position = levels[position].transform.position + offset;
        movement = false;
        HideLevels();
        levelDisplay.text = levels[position].name;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!active) return;

        if (!movement)
        {
            if (inputsManager.GetButton("Fire3"))
            {
                MoveRight();
            }
            else if (inputsManager.GetButton("Fire1"))
            {
                MoveLeft();
            }
            if (inputsManager.GetButtonDown("Fire2"))
            {
                StartCoroutine(EnterLevel());
            }

        }
        if (movement)
        {
            MoveTowardDestination();
        }
    }

    public void SetActive(bool state)
    {
        active = state;
    }

    private IEnumerator EnterLevel()
    {
        SetActive(false);
        float time = enterLevelClip.length;
        audioSource.PlayOneShot(enterLevelClip, 1.0f);

        yield return new WaitForSeconds(time);

        ApplicationDatas.Game.CurrentLevel = position;
        SceneManager.LoadScene("Levels");
    }

    private void Move(bool state)
    {
        movement = state;
        animator.SetBool("Walking", state);
        if (state)
        {
            audioSource.PlayOneShot(levelSelect, 1.0f);
            levelDisplay.enabled = false;
        }
        else
        {
            levelDisplay.enabled = true;
            levelDisplay.text = levels[position].name;
        }
    }

    private void MoveLeft()
    {
        if (position >= 1)
        {
            Move(true);
            destination = levels[--position];
        }
    }

    private void MoveRight()
    {
        if (position < levels.Count - 1 && position < levelCleared)
        {
            Move(true);
            destination = levels[++position];
        }
    }

    private void MoveTowardDestination()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination.transform.position + offset, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, destination.transform.position + offset) == 0)
        {
            Move(false);
        }
    }

    private void HideLevels()
    {
        for (int i = levelCleared + 1; i < levels.Count; ++i)
        {
            levels[i].GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
