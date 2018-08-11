using System;
using UnityEngine;

namespace Resources
{
    [Serializable]
    public struct Energy : IEquatable<Energy>, IComparable<Energy>
    {
        public static readonly Energy Zero = new Energy();
        
        [SerializeField]
        private float value;

        public Energy(float value)
        {
            this.value = value;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(Energy other)
        {
            return value.Equals(other.value);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public static bool operator ==(Energy left, Energy right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Energy left, Energy right)
        {
            return !left.Equals(right);
        }

        public static Energy operator +(Energy left, Energy right)
        {
            return new Energy(left.value + right.value);
        }

        public static Energy operator -(Energy left, Energy right)
        {
            return new Energy(left.value - right.value);
        }

        public static Energy operator -(Energy energy)
        {
            return new Energy(-energy.value);
        }
        
        public static Energy operator *(Energy left, float right)
        {
            return new Energy(left.value * right);
        }
        
        public static Energy operator *(float left, Energy right)
        {
            return new Energy(left * right.value);
        }
        
        public static Energy operator /(Energy left, float right)
        {
            return new Energy(left.value / right);
        }
        
        public static float operator /(Energy left, Energy right)
        {
            return left.value / right.value;
        }
        
        public static bool operator <(Energy left, Energy right)
        {
            return left.value < right.value;
        }

        public static bool operator >(Energy left, Energy right)
        {
            return left.value > right.value;
        }
        
        public static bool operator <=(Energy left, Energy right)
        {
            return left.value <= right.value;
        }

        public static bool operator >=(Energy left, Energy right)
        {
            return left.value >= right.value;
        }

        public static explicit operator float(Energy energy)
        {
            return energy.value;
        }

        public int CompareTo(Energy other)
        {
            return value.CompareTo(other.value);
        }
    }
}