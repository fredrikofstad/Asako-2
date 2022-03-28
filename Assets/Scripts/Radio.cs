using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    private ParticleSystem notes;
    private AudioSource podcast;
    private bool started;
    ParticleSystem.EmissionModule emission;


    private void Start()
    {
        notes = GetComponentInChildren<ParticleSystem>();
        podcast = GetComponent<AudioSource>();
        emission = notes.emission;
        emission.enabled = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !GameManager.instance.isAudioPlaying)
        {
            GameManager.instance.isAudioPlaying = true;
            started = true;
            PlayAudio();
        }
    }

    private void Update()
    {
        if (!started) return;
        if (!podcast.isPlaying)
        {
            emission.enabled = false;
            GameManager.instance.isAudioPlaying = false;
            started = false;
        }
    }


    private void PlayAudio()
    {
        podcast.Play();
        emission.enabled = true;
    }
}
