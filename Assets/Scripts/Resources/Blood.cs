using System;
using UnityEngine;

namespace Resources
{
    [Serializable]
    public struct Blood : IEquatable<Blood>, IComparable<Blood>, IComparable
    {
        public static readonly Blood Zero = new Blood();

        [SerializeField]
        private float value;

        public Blood(float value)
        {
            this.value = value;
        }

        public bool Equals(Blood other)
        {
            return value.Equals(other.value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Blood && Equals((Blood) obj);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public static bool operator ==(Blood left, Blood right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Blood left, Blood right)
        {
            return !left.Equals(right);
        }

        public static Blood operator +(Blood left, Blood right)
        {
            return new Blood(left.value + right.value);
        }

        public static Blood operator -(Blood left, Blood right)
        {
            return new Blood(left.value - right.value);
        }

        public static Blood operator -(Blood blood)
        {
            return new Blood(-blood.value);
        }
        
        public static Blood operator *(Blood left, float right)
        {
            return new Blood(left.value * right);
        }
        
        public static Blood operator *(float left, Blood right)
        {
            return new Blood(left * right.value);
        }
        
        public static Blood operator /(Blood left, float right)
        {
            return new Blood(left.value / right);
        }
        
        public static float operator /(Blood left, Blood right)
        {
            return left.value / right.value;
        }

        public int CompareTo(Blood other)
        {
            return value.CompareTo(other.value);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (!(obj is Blood)) throw new ArgumentException($"Object must be of type {nameof(Blood)}");
            return CompareTo((Blood) obj);
        }

        public static bool operator <(Blood left, Blood right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(Blood left, Blood right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(Blood left, Blood right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(Blood left, Blood right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static explicit operator float(Blood blood)
        {
            return blood.value;
        }
    }
}