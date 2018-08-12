using UnityEngine;

public class Pistole : MonoBehaviour, IPlayerWeapon
{
    [SerializeField]
    [Tooltip("The bullet Prefab")]
    private GameObject bulletPrefab;

    [SerializeField]
    [Tooltip("The bullet spawn point. Should be preconfigured in the Prefab")]
    private GameObject bulletSpawn;

    public void Fire()
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().AddForce(-bulletSpawn.transform.right * 10);
    }
}