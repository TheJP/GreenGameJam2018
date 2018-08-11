using System;
using System.Runtime.Serialization;

namespace Monsters
{
    [Serializable]
    public struct MonsterAttributes
    {
        public int Health;
        public int Attackpower;
        public float MoveForce;
        public float Range;
        public float JumpForce;

        public int EnergyValue;
        public float MaxSpeed;
    }
}