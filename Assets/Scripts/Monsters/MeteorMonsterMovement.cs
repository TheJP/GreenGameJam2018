using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monsters
{
    public class MeteorMonsterMovement : MonsterMovement
    {

        public int speed;
        
        // Use this for initialization
        void Start () {
		
        }
	
        // Update is called once per frame
        void Update () {
		
        }

        public override void MoveTowards(AttackableBuilding attackableBuilding)
        {
            
            throw new System.NotImplementedException();
        }

        public override bool CanOnlyFall()
        {
            return true;
        }
    }

}

