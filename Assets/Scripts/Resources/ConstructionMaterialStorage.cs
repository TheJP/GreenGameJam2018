using UnityEngine;

namespace Resources
{
    public class ConstructionMaterialStorage : MonoBehaviour
    {
        [SerializeField]
        private ConstructionMaterial capacity;
        
        [SerializeField]
        private ConstructionMaterial currentValue;

        public ConstructionMaterial Capacity => capacity;
        public ConstructionMaterial CurrentValue => currentValue;

        public ConstructionMaterialStorage()
        {
        }

        public ConstructionMaterialStorage(ConstructionMaterial capacity)
        {
            this.capacity = capacity;
        }

        /// <summary>
        /// Changes the maximum storage capacity.
        /// </summary>
        /// <param name="newCapacity">The new capacity</param>
        /// <returns>The amount of overflow if the current value needs to be adjusted.</returns>
        public ConstructionMaterial ChangeMaxStorage(ConstructionMaterial newCapacity)
        {
            if (currentValue <= newCapacity)
            {
                capacity = newCapacity;
                return new ConstructionMaterial();
            }

            var overflow = currentValue - newCapacity;
            currentValue = newCapacity;
            return overflow;
        }

        public ConstructionMaterial Consume(ConstructionMaterial value)
        {
            if (value < new ConstructionMaterial())
            {
                return -Store(-value);
            }

            if (currentValue - value < new ConstructionMaterial())
            {
                value -= currentValue;
                currentValue = new ConstructionMaterial();
                return value;
            }

            currentValue -= value;
            return new ConstructionMaterial();
        }

        public ConstructionMaterial Store(ConstructionMaterial value)
        {
            if (value < new ConstructionMaterial())
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
            return new ConstructionMaterial();
        }
    }
}