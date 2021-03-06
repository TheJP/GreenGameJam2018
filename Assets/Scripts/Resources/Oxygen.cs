﻿using System;
using UnityEngine;

namespace Resources
{
    [Serializable]
    public struct Oxygen : IEquatable<Oxygen>, IComparable<Oxygen>, IComparable
    {
        public static readonly Oxygen Zero = new Oxygen();
        
        [SerializeField]
        private float value;

        public Oxygen(float value)
        {
            this.value = value;
        }

        public bool Equals(Oxygen other)
        {
            return value.Equals(other.value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Oxygen && Equals((Oxygen) obj);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public static bool operator ==(Oxygen left, Oxygen right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Oxygen left, Oxygen right)
        {
            return !left.Equals(right);
        }

        public static Oxygen operator +(Oxygen left, Oxygen right)
        {
            return new Oxygen(left.value + right.value);
        }

        public static Oxygen operator -(Oxygen left, Oxygen right)
        {
            return new Oxygen(left.value - right.value);
        }

        public static Oxygen operator -(Oxygen oxygen)
        {
            return new Oxygen(-oxygen.value);
        }
        
        public static Oxygen operator *(Oxygen left, float right)
        {
            return new Oxygen(left.value * right);
        }
        
        public static Oxygen operator *(float left, Oxygen right)
        {
            return new Oxygen(left * right.value);
        }
        
        public static Oxygen operator /(Oxygen left, float right)
        {
            return new Oxygen(left.value / right);
        }
        
        public static float operator /(Oxygen left, Oxygen right)
        {
            return left.value / right.value;
        }

        public int CompareTo(Oxygen other)
        {
            return value.CompareTo(other.value);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (!(obj is Oxygen)) throw new ArgumentException($"Object must be of type {nameof(Oxygen)}");
            return CompareTo((Oxygen) obj);
        }

        public static bool operator <(Oxygen left, Oxygen right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(Oxygen left, Oxygen right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(Oxygen left, Oxygen right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(Oxygen left, Oxygen right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static explicit operator float(Oxygen oxygen)
        {
            return oxygen.value;
        }
    }
}