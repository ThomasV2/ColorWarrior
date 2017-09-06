using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {

    [SerializeField]
    private int level;
    [SerializeField]
    private List<AudioClip> bgms;
    [SerializeField]
    private AudioClip deathClip;

    private AudioSource audioSource;
    private bool fadeIn;
    private bool fadeOut;
    private float step;

	// Use this for initialization
	void Start ()
    {
        // Si ApplicationDatas.Level vaut -1, on ne vient pas de l'overworld donc on est en debug
        if (ApplicationDatas.Game.CurrentLevel >= 0)
        {
            level = ApplicationDatas.Game.CurrentLevel;
            if (level >= bgms.Count)
            {
                level = bgms.Count - 1;
            }
        }
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = bgms[level];
        audioSource.Play();
    }

    void Update()
    {
        if (fadeOut && audioSource.volume > 0)
        {
            audioSource.volume -= step * Time.deltaTime;
        }
        if (fadeIn && audioSource.volume < 1)
        {
            audioSource.volume += step * Time.deltaTime;
        }
    }

    public void PlayBack()
    {
        audioSource.clip = bgms[level];
        audioSource.Play();
    }

    public void PlayClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlayDeath()
    {
        audioSource.clip = deathClip;
        audioSource.Play();
    }

    public void FadeOut(float time)
    {
        step = 1.0f / time;
        fadeIn = false;
        fadeOut = true;
    }

    public void FadeIn(float time)
    {
        step = 1.0f / time;
        fadeOut = false;
        fadeIn = true;
    }
}
