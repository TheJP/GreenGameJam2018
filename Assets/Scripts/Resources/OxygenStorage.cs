using UnityEngine;

namespace Resources
{
    public class OxygenStorage
    {
        [SerializeField]
        private Oxygen capacity;
        
        [SerializeField]
        private Oxygen currentValue;

        public Oxygen Capacity => capacity;
        public Oxygen CurrentValue => currentValue;

        public OxygenStorage()
        {
        }

        public OxygenStorage(Oxygen capacity)
        {
            this.capacity = capacity;
        }

        /// <summary>
        /// Changes the maximum storage capacity.
        /// </summary>
        /// <param name="newCapacity">The new capacity</param>
        /// <returns>The amount of overflow if the current value needs to be adjusted.</returns>
        public Oxygen ChangeMaxStorage(Oxygen newCapacity)
        {
            if (currentValue <= newCapacity)
            {
                capacity = newCapacity;
                return new Oxygen();
            }

            var overflow = currentValue - newCapacity;
            currentValue = newCapacity;
            return overflow;
        }

        public Oxygen Consume(Oxygen value)
        {
            if (value < new Oxygen())
            {
                return -Store(-value);
            }

            if (currentValue - value < new Oxygen())
            {
                value -= currentValue;
                currentValue = new Oxygen();
                return value;
            }

            currentValue -= value;
            return new Oxygen();
        }

        public Oxygen Store(Oxygen value)
        {
            if (value < new Oxygen())
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
            return new Oxygen();
        }
    }
}