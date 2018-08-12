using UnityEngine;

public class Pistole : MonoBehaviour, IPlayerWeapon
{
    [SerializeField]
    [Tooltip("The bullet Prefab")]
    private GameObject bulletPrefab;

    [SerializeField]
    [Tooltip("The bullet spawn point. Should be preconfigured in the Prefab")]
    private GameObject bulletSpawn;

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
        var bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().AddForce(-bulletSpawn.transform.right * 10);

        audioSource.Play();
    }
}