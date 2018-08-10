using UnityEngine;

namespace Resources
{
    public class EnergyStorage : MonoBehaviour
    {
        [SerializeField]
        private Energy capacity;
        
        [SerializeField]
        private Energy currentValue;

        public Energy Capacity => capacity;
        public Energy CurrentValue => currentValue;
        
        public EnergyStorage()
        {
        }

        public EnergyStorage(Energy capacity)
        {
            this.capacity = capacity;
        }

        /// <summary>
        /// Changes the maximum storage capacity.
        /// </summary>
        /// <param name="newCapacity">The new capacity</param>
        /// <returns>The amount of overflow if the current value needs to be adjusted.</returns>
        public Energy ChangeMaxStorage(Energy newCapacity)
        {
            if (currentValue <= newCapacity)
            {
                capacity = newCapacity;
                return new Energy();
            }

            var overflow = currentValue - newCapacity;
            currentValue = newCapacity;
            return overflow;
        }

        public Energy Consume(Energy value)
        {
            if (value < new Energy())
            {
                return -Store(-value);
            }

            if (currentValue - value < new Energy())
            {
                value -= currentValue;
                currentValue = new Energy();
                return value;
            }

            currentValue -= value;
            return new Energy();
        }

        public Energy Store(Energy value)
        {
            if (value < new Energy())
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
            return new Energy();
        }
    }
}