using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanSound : MonoBehaviour
{
    public AudioClip normalCollisionSound;
    public AudioClip projectileCollisionSound;
    private AudioSource audioSource;
    private bool canPlaySound = false;
    private float startTime;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        startTime = Time.time;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (canPlaySound && collision.relativeVelocity.magnitude > 0.1f)
        {
            if (collision.gameObject.CompareTag("Projectile"))
            {
                PlaySound(projectileCollisionSound);
            }
            else
            {
                PlaySound(normalCollisionSound);
            }
        }
    }

    void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    void Update()
    {
        if (Time.time - startTime > 0.5f)
        {
            canPlaySound = true;
        }
    }
}
