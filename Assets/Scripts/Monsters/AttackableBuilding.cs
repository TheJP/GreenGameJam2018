using UnityEngine;

namespace Monsters
{
    public class AttackableBuilding : MonoBehaviour
    {
        public float health = 200f;

        public void TakeDamage(float damage)
        {
            health -= damage;
        }
    }
}