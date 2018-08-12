using UnityEngine;

public class Pistole : MonoBehaviour, IPlayerWeapon
{

#pragma warning disable 0649
    [SerializeField]
    [Tooltip("The bullet Prefab")]
    private GameObject bulletPrefab;

    [SerializeField]
    [Tooltip("The bullet spawn point. Should be preconfigured in the Prefab")]
    private GameObject bulletSpawn;

    [SerializeField]
    private AudioSource audioSource;
#pragma warning restore 0649

    [SerializeField]
    [Tooltip("Shooting power of the gun")]
    private int shootingPower = 20;

    private float originalPitch;
    private float pitchRange = 0.2f;


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

    public void Fire()
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().AddForce(bulletSpawn.transform.right * shootingPower);

        audioSource.Play();
    }
}