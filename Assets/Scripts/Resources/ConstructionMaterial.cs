using System;
using UnityEngine;

namespace Resources
{
    [Serializable]
    public struct ConstructionMaterial : IEquatable<ConstructionMaterial>, IComparable<ConstructionMaterial>, IComparable
    {
        public static readonly ConstructionMaterial Zero = new ConstructionMaterial();
        
        [SerializeField]
        private float value;

        public ConstructionMaterial(float value)
        {
            this.value = value;
        }

        public bool Equals(ConstructionMaterial other)
        {
            return value.Equals(other.value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ConstructionMaterial && Equals((ConstructionMaterial) obj);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public static bool operator ==(ConstructionMaterial left, ConstructionMaterial right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ConstructionMaterial left, ConstructionMaterial right)
        {
            return !left.Equals(right);
        }

        public static ConstructionMaterial operator +(ConstructionMaterial left, ConstructionMaterial right)
        {
            return new ConstructionMaterial(left.value + right.value);
        }

        public static ConstructionMaterial operator -(ConstructionMaterial left, ConstructionMaterial right)
        {
            return new ConstructionMaterial(left.value - right.value);
        }

        public static ConstructionMaterial operator -(ConstructionMaterial material)
        {
            return new ConstructionMaterial(-material.value);
        }

        public int CompareTo(ConstructionMaterial other)
        {
            return value.CompareTo(other.value);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (!(obj is ConstructionMaterial)) throw new ArgumentException($"Object must be of type {nameof(ConstructionMaterial)}");
            return CompareTo((ConstructionMaterial) obj);
        }

        public static bool operator <(ConstructionMaterial left, ConstructionMaterial right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(ConstructionMaterial left, ConstructionMaterial right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(ConstructionMaterial left, ConstructionMaterial right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(ConstructionMaterial left, ConstructionMaterial right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static explicit operator float(ConstructionMaterial material)
        {
            return material.value;
        }
    }
}