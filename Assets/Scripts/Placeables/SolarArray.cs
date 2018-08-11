using Resources;
using UnityEngine;

public class SolarArray : MonoBehaviour, EnergySource
{
    #pragma warning disable 0649
    [SerializeField]
    private Energy energyPerSecond;
    #pragma warning restore 0649

    public void ProduceEnergy(ResourceManager manager)
    {
        manager.Store(energyPerSecond * Time.deltaTime);
    }
}