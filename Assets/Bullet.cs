using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The Damge the bullet makes")]
    private float damage = 5;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Health>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
