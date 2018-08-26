using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;

namespace Monsters
{
    public class MeteorMonsterMovement : MonsterMovement
    {
        
        public float speed = 8.0f;
        
        // Use this for initialization
        void Start () {
		    
        }
	
        public override void MoveTowards(AttackableBuilding attackableBuilding)
        {
            // move our position a step closer to the target
            float step = speed * Time.fixedDeltaTime;    // fixedDeltaTime because MoveTowards is called in FixedUpdate
            transform.position = Vector3.MoveTowards(transform.position, attackableBuilding.transform.position, step);
        }

        public override bool CanOnlyFall()
        {
            return true;
        }
    }

}

