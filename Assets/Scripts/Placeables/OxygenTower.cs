using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resources;

public class OxygenTower : MonoBehaviour, OxygenSink, EnergySink
{
    public void ConsumeEnergy(ResourceManager manager)
    {
        throw new System.NotImplementedException();
    }

    public void ConsumeOxygen(ResourceManager manager)
    {

    }
}
