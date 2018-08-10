using UnityEngine;

namespace Resources
{
    public class BloodStorage : MonoBehaviour
    {
        [SerializeField]
        private Blood capacity;
        
        [SerializeField]
        private Blood currentValue;

        public Blood Capacity => capacity;
        public Blood CurrentValue => currentValue;

        public BloodStorage()
        {
        }

        public BloodStorage(Blood capacity)
        {
            this.capacity = capacity;
        }

        /// <summary>
        /// Changes the maximum storage capacity.
        /// </summary>
        /// <param name="newCapacity">The new capacity</param>
        /// <returns>The amount of overflow if the current value needs to be adjusted.</returns>
        public Blood ChangeMaxStorage(Blood newCapacity)
        {
            if (currentValue <= newCapacity)
            {
                capacity = newCapacity;
                return new Blood();
            }

            var overflow = currentValue - newCapacity;
            currentValue = newCapacity;
            return overflow;
        }

        public Blood Consume(Blood value)
        {
            if (value < new Blood())
            {
                return -Store(-value);
            }

            if (currentValue - value < new Blood())
            {
                value -= currentValue;
                currentValue = new Blood();
                return value;
            }

            currentValue -= value;
            return new Blood();
        }

        public Blood Store(Blood value)
        {
            if (value < new Blood())
            {
                return -Consume(-value);
            }
            
            if (currentValue + value > capacity)
            {
                value -= capacity - currentValue;
                currentValue = capacity;
                return value;
            }

            currentValue += value;
            return new Blood();
        }
    }
}