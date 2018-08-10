using System;
using System.Runtime.Serialization;

namespace Monsters
{
    [Serializable]
    public struct MonsterAttributes
    {
        public int Health;
        public int Attackpower;
        public int Speed;
        
        public int EnergyValue;
    }
}