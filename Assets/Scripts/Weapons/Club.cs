using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Club : MonoBehaviour, IPlayerWeapon
{
#pragma warning disable 0649
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private Collider2D damageCollider;
#pragma warning restore 0649

    [SerializeField]
    private float damage = 20;

    private float originalPitch;
    private float pitchRange = 0.2f;
    private const float ColiderResetTime = 0.5F;
    private float coliderResetTimer;

    private void Awake()
    {
        if (audioSource != null)
        {
            // audioSource.volume = 0.9f;
            audioSource.loop = false;
            audioSource.playOnAwake = false;
            originalPitch = audioSource.pitch;
            audioSource.pitch = UnityEngine.Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
        }
    }

    private void Update()
    {
        this.coliderResetTimer += Time.deltaTime;
        if (coliderResetTimer > ColiderResetTime)
        {
            coliderResetTimer = 0.0F;
            damageCollider.enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Health>()?.TakeDamage(damage);
            damageCollider.enabled = false;
        }
    }

    public void Fire()
    {
        animator.SetTrigger("beat");
        audioSource.Play();
        damageCollider.enabled = true;
    }
}