using UnityEngine;

namespace Monsters
{
    public class Building : MonoBehaviour
    {
        private float health;

        public void TakeDamage(float damage)
        {
            health -= damage;
        }
    }
}