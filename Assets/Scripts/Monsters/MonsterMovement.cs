using UnityEngine;

namespace Monsters
{
    public abstract class MonsterMovement : MonoBehaviour
    {
        /// <summary>
        /// Call in FixedUpdate to Move the monster
        /// </summary>
        /// <param name="attackableBuilding"></param>
        public abstract void MoveTowards(AttackableBuilding attackableBuilding);
    }
}