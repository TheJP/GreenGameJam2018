using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Club : MonoBehaviour, IPlayerWeapon
{
    [SerializeField]
    private Animator animator;

    private AudioSource audioSource;
    private float originalPitch;
    private float pitchRange = 0.2f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            // audioSource.volume = 0.9f;
            audioSource.loop = true;
            audioSource.playOnAwake = false;
            originalPitch = audioSource.pitch;
            audioSource.pitch = UnityEngine.Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);                
        }
    }
    
    public void Fire()
    {
        animator.SetTrigger("beat");

        audioSource.Play();

        //TODO: Make Damge to Mobs
    }
}