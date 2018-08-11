using Resources;
using UnityEngine;

public class SolarArray : Placeable, EnergySource
{
    #pragma warning disable 0649
    [SerializeField]
    private Energy energyPerSecond;

    [SerializeField]
    private Sprite solarOffSprite;
    
    [SerializeField]
    private Sprite solarOnSprite;
    
    [SerializeField]
    private bool isSolarOn;
    #pragma warning restore 0649

    public bool IsSolarOn
    {
        get { return isSolarOn; }
        set { isSolarOn = value; }
    }

    private SpriteRenderer spriteRenderer;
    private ResourceManager resourceManager;
    private EnergyStorage energyStorage;

    protected override void Start()
    {
        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();
        resourceManager = FindObjectOfType<ResourceManager>();
        energyStorage = GetComponent<EnergyStorage>();
        
        resourceManager.AddSource(this);
        resourceManager.AddStorage(energyStorage);
    }

    private void Update()
    {
        spriteRenderer.sprite = IsSolarOn ? solarOnSprite : solarOffSprite;
    }

    private void OnDestroy()
    {
        resourceManager.RemoveSource(this);
        resourceManager.RemoveStorage(energyStorage);
    }

    public void ProduceEnergy(ResourceManager manager)
    {
        if (IsSolarOn)
        {
            manager.Store(energyPerSecond * Time.deltaTime);
        }
    }
}