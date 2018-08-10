using System;
using Boo.Lang;
using UnityEngine;

namespace Resources
{
    public class ResourceManager : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The amount of energy available")]
        private Energy energyStorage;
        
        [SerializeField]
        [Tooltip("The amount of oxygen available")]
        private Oxygen oxygenStorage;

        [SerializeField]
        [Tooltip("The amount of construction material available")]
        private ConstructionMaterial constructionMaterialStorage;

        private readonly List<EnergySink> energySinks;
        private readonly List<EnergySource> energySources;
        private readonly List<OxygenSink> oxygenSinks;
        private readonly List<OxygenSource> oxygenSources;
        private readonly List<ConstructionMaterialSink> constructionMaterialSinks;
        private readonly List<ConstructionMaterialSource> constructionMaterialSources;

        public Energy EnergyAvailable => energyStorage;
        public Oxygen OxygenAvailable => oxygenStorage;
        public ConstructionMaterial ConstructionMaterialAvailable => constructionMaterialStorage;

        public ResourceManager()
        {
            energySinks = new List<EnergySink>();
            energySources = new List<EnergySource>();
            oxygenSinks = new List<OxygenSink>();
            oxygenSources = new List<OxygenSource>();
            constructionMaterialSinks = new List<ConstructionMaterialSink>();
            constructionMaterialSources = new List<ConstructionMaterialSource>();
        }

        public void Store(Energy energy)
        {
            energyStorage += energy;
        }
        
        public void Store(Oxygen oxygen)
        {
            oxygenStorage += oxygen;
        }

        public void Store(ConstructionMaterial material)
        {
            constructionMaterialStorage += material;
        }
        
        public bool TryConsume(Energy energy)
        {
            if (energy > energyStorage)
            {
                return false;
            }

            energyStorage -= energy;
            return true;
        }
        
        public bool TryConsume(Oxygen oxygen)
        {
            if (oxygen > oxygenStorage)
            {
                return false;
            }

            oxygenStorage -= oxygen;
            return true;
        }

        public bool TryConsume(ConstructionMaterial material)
        {
            if (material > constructionMaterialStorage)
            {
                return false;
            }

            constructionMaterialStorage -= material;
            return true;
        }

        public void AddSource(EnergySource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            energySources.Add(source);
        }

        public void AddSource(OxygenSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            oxygenSources.Add(source);
        }

        public void AddSource(ConstructionMaterialSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            constructionMaterialSources.Add(source);
        }

        public void RemoveSource(EnergySource source)
        {
            energySources.Remove(source);
        }

        public void RemoveSource(OxygenSource source)
        {
            oxygenSources.Remove(source);
        }

        public void RemoveSource(ConstructionMaterialSource source)
        {
            constructionMaterialSources.Remove(source);
        }

        public void AddSink(EnergySink sink)
        {
            if (sink == null)
            {
                throw new ArgumentNullException(nameof(sink));
            }

            energySinks.Add(sink);
        }

        public void AddSink(OxygenSink sink)
        {
            if (sink == null)
            {
                throw new ArgumentNullException(nameof(sink));
            }

            oxygenSinks.Add(sink);
        }

        public void AddSink(ConstructionMaterialSink sink)
        {
            if (sink == null)
            {
                throw new ArgumentNullException(nameof(sink));
            }

            constructionMaterialSinks.Add(sink);
        }

        public void RemoveSink(EnergySink sink)
        {
            energySinks.Remove(sink);
        }

        public void RemoveSink(OxygenSink sink)
        {
            oxygenSinks.Remove(sink);
        }

        public void RemoveSink(ConstructionMaterialSink sink)
        {
            constructionMaterialSinks.Remove(sink);
        }

        private void FixedUpdate()
        {
            Process();
        }

        /// <summary>
        /// Produces energy, consume energy, produces oxygen, consumes oxygen, produces construction material, consumes
        /// construction material. In this exact order.
        /// </summary>
        private void Process()
        {
            foreach (var source in energySources)
            {
                source.ProduceEnergy(this);
            }
            
            foreach (var sink in energySinks)
            {
                sink.ConsumeEnergy(this);
            }
            
            foreach (var source in oxygenSources)
            {
                source.ProduceOxygen(this);
            }
            
            foreach (var sink in oxygenSinks)
            {
                sink.ConsumeOxygen(this);
            }
            
            foreach (var source in constructionMaterialSources)
            {
                source.ProduceMaterial(this);
            }
            
            foreach (var sink in constructionMaterialSinks)
            {
                sink.ConsumeMaterial(this);
            }
        }
    }
}
