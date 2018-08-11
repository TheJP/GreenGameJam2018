using Resources;
using UnityEngine;

public class SolarArray : Placeable, EnergySource
{
    #pragma warning disable 0649
    [SerializeField]
    private Energy energyPerSecond;
    #pragma warning restore 0649

    private ResourceManager resourceManager;
    private EnergyStorage energyStorage;

    protected override void Start()
    {
        base.Start();

        resourceManager = FindObjectOfType<ResourceManager>();
        energyStorage = GetComponent<EnergyStorage>();
        
        resourceManager.AddSource(this);
        resourceManager.AddStorage(energyStorage);
    }

    private void OnDestroy()
    {
        resourceManager.RemoveSource(this);
        resourceManager.RemoveStorage(energyStorage);
    }

    public void ProduceEnergy(ResourceManager manager)
    {
        manager.Store(energyPerSecond * Time.deltaTime);
    }
}