using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip death;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void JumpSound()
    {
        audioSource.PlayOneShot(jump);
    }

    public void DeathSound()
    {
        audioSource.PlayOneShot(death);
    }

   
    
}
