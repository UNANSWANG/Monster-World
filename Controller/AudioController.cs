using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [Header("== “Ù–ß ==")]
    public AudioClip attack1;
    public AudioClip attack2;
    public AudioClip attack3;

    public AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void Attack_1_Audio()
    {
        audioSource.clip = attack1;
        audioSource.Play();
    }

    public void Attack_2_Audio()
    {
        audioSource.clip = attack2;
        audioSource.Play();
    }

    public void Attack_3_Audio()
    {
        audioSource.clip = attack3;
        audioSource.Play();
    }
}
