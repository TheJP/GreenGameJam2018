using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resources;

public class Quarry : Placeable, EnergySink, ConstructionMaterialSource
{
    [Tooltip("Amount of construction material that is collected per second")]
    public ConstructionMaterial constructionMaterialPerSecond = new ConstructionMaterial(10f);

    [Tooltip("Amount of energy used per second")]
    public Energy energyUsedPerSecond = new Energy(5f);

    [Tooltip("The quarry will at least collect this amount before it delivers it to the main resource pool")]
    public ConstructionMaterial minimumDeliveryAmount = new ConstructionMaterial(20f);

    [Tooltip("Size of the internal storage of this quarry")]
    public ConstructionMaterial internalStorageSize = new ConstructionMaterial(50f);

    [Tooltip("Prefab to show resource gathering information")]
    public FadeoutText fadeoutTextPrefab;

    private ResourceManager manager;

    public bool HasEnergy { get; private set; } = false;

    public ConstructionMaterial InternalStorage { get; private set; } = ConstructionMaterial.Zero;


    protected override void Start()
    {
        base.Start();

        manager = FindObjectOfType<ResourceManager>();
        manager.AddSink(this);
        manager.AddSource(this);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        manager.RemoveSink(this);
        manager.RemoveSource(this);
    }

    public void ConsumeEnergy(ResourceManager manager) =>
        HasEnergy = manager.TryConsume(energyUsedPerSecond * Time.fixedDeltaTime);

    public void ProduceMaterial(ResourceManager manager)
    {
        if (HasEnergy)
        {
            InternalStorage += constructionMaterialPerSecond * Time.fixedDeltaTime;
            InternalStorage = ResourceMath.Min(internalStorageSize, InternalStorage);
            Debug.Assert(internalStorageSize >= minimumDeliveryAmount, "Quarry internal storage is not big enough");
            Debug.Assert(minimumDeliveryAmount > ConstructionMaterial.Zero, "Delivery amount has to be bigger than 0");
            while (InternalStorage >= minimumDeliveryAmount)
            {
                manager.Store(minimumDeliveryAmount);
                InternalStorage -= minimumDeliveryAmount;
                var fadeout = Instantiate(fadeoutTextPrefab, transform);
                fadeout.TextMesh.text = $"+{(float)minimumDeliveryAmount:F0}";
                fadeout.TextMesh.color = new Color(1f, 0.5f, 0.15f, 1);
            }
        }
    }
}
