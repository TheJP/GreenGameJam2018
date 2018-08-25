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

        // return true if this movement is passive (like for the meteors)
        public abstract bool CanOnlyFall();
    }
}