using System;
using System.Linq;
using Boo.Lang;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Resources
{
    public class ResourceManager : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The energy base storage")]
        private EnergyStorage energyBaseStorage;
        
        [SerializeField]
        [Tooltip("The oxygen base storage")]
        private OxygenStorage oxygenBaseStorage;

        [SerializeField]
        [Tooltip("The construction material base storage")]
        private ConstructionMaterialStorage constructionMaterialBaseStorage;

        private readonly List<EnergySink> energySinks;
        private readonly List<EnergySource> energySources;
        private readonly List<EnergyStorage> energyStorages;
        
        private readonly List<OxygenSink> oxygenSinks;
        private readonly List<OxygenSource> oxygenSources;
        private readonly List<OxygenStorage> oxygenStorages;
        
        private readonly List<ConstructionMaterialSink> constructionMaterialSinks;
        private readonly List<ConstructionMaterialSource> constructionMaterialSources;
        private readonly List<ConstructionMaterialStorage> constructionMaterialStorages;

        public EnergyStorage EnergyBaseStorage => energyBaseStorage;
        public OxygenStorage OxygenBaseStorage => oxygenBaseStorage;
        public ConstructionMaterialStorage ConstructionMaterialBaseStorage => constructionMaterialBaseStorage;

        public Energy EnergyAvailable => energyStorages.Aggregate(energyBaseStorage.CurrentValue, (a, s) => a + s.CurrentValue);
        public Oxygen OxygenAvailable => oxygenStorages.Aggregate(oxygenBaseStorage.CurrentValue, (a, s) => a + s.CurrentValue);
        public ConstructionMaterial ConstructionMaterialAvailable => constructionMaterialStorages.Aggregate(constructionMaterialBaseStorage.CurrentValue, (a, s) => a + s.CurrentValue);

        public ResourceManager()
        {
            energySinks = new List<EnergySink>();
            energySources = new List<EnergySource>();
            energyStorages = new List<EnergyStorage>();
            
            oxygenSinks = new List<OxygenSink>();
            oxygenSources = new List<OxygenSource>();
            oxygenStorages = new List<OxygenStorage>();
            
            constructionMaterialSinks = new List<ConstructionMaterialSink>();
            constructionMaterialSources = new List<ConstructionMaterialSource>();
            constructionMaterialStorages = new List<ConstructionMaterialStorage>();
        }

        public void Store(Energy energy)
        {
            energy = energyBaseStorage.Store(energy);

            using (var storages = energyStorages.GetEnumerator())
            {
                while (energy > Energy.Zero && storages.MoveNext())
                {
                    Debug.Assert(storages.Current != null, "storages.Current != null");
                    energy = storages.Current.Store(energy);
                }
            }
        }
        
        public void Store(Oxygen oxygen)
        {
            oxygen = oxygenBaseStorage.Store(oxygen);

            using (var storages = oxygenStorages.GetEnumerator())
            {
                while (oxygen > Oxygen.Zero && storages.MoveNext())
                {
                    Debug.Assert(storages.Current != null, "storages.Current != null");
                    oxygen = storages.Current.Store(oxygen);
                }
            }
        }

        public void Store(ConstructionMaterial material)
        {
            material = constructionMaterialBaseStorage.Store(material);

            using (var storages = constructionMaterialStorages.GetEnumerator())
            {
                while (material > ConstructionMaterial.Zero && storages.MoveNext())
                {
                    Debug.Assert(storages.Current != null, "storages.Current != null");
                    material = storages.Current.Store(material);
                }
            }
        }
        
        public bool TryConsume(Energy energy)
        {
            if (energy > EnergyAvailable)
            {
                return false;
            }

            energy = energyBaseStorage.Consume(energy);
            foreach (var storage in energyStorages)
            {
                energy = storage.Consume(energy);
            }
            
            return true;
        }
        
        public bool TryConsume(Oxygen oxygen)
        {
            if (oxygen > OxygenAvailable)
            {
                return false;
            }

            oxygen = oxygenBaseStorage.Consume(oxygen);
            foreach (var storage in oxygenStorages)
            {
                oxygen = storage.Consume(oxygen);
            }
            
            return true;
        }

        public bool TryConsume(ConstructionMaterial material)
        {
            if (material > ConstructionMaterialAvailable)
            {
                return false;
            }

            material = constructionMaterialBaseStorage.Consume(material);
            foreach (var storage in constructionMaterialStorages)
            {
                material = storage.Consume(material);
            }
            
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

        public void AddStorage(EnergyStorage storage)
        {
            if (storage == null)
            {
                throw new ArgumentNullException(nameof(storage));
            }

            energyStorages.Add(storage);
        }

        public void AddStorage(OxygenStorage storage)
        {
            if (storage == null)
            {
                throw new ArgumentNullException(nameof(storage));
            }

            oxygenStorages.Add(storage);
        }

        public void AddStorage(ConstructionMaterialStorage storage)
        {
            if (storage == null)
            {
                throw new ArgumentNullException(nameof(storage));
            }

            constructionMaterialStorages.Add(storage);
        }

        public void RemoveStorage(EnergyStorage storage)
        {
            energyStorages.Remove(storage);
        }

        public void RemoveStorage(OxygenStorage storage)
        {
            oxygenStorages.Remove(storage);
        }

        public void RemoveStorage(ConstructionMaterialStorage storage)
        {
            constructionMaterialStorages.Remove(storage);
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
