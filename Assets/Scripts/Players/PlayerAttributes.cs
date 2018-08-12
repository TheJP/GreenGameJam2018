using Resources;
using System;

public class PlayerAttributes
{
    public Oxygen MaxOxygen { get; set; }
    public Oxygen CurrentOxygen { get; private set; }
    public bool IsAlive { get; set; }

    public event Action<Oxygen> OxygenLevelChanged;

    public IInventoryItem CurrentEquippedItem { get; set; }

    //List of Phobies
    //Weapon Inventory
    //Tower Inventory

    public PlayerAttributes(Oxygen maxOxygen)
    {
        MaxOxygen = maxOxygen;
        this.CurrentOxygen = maxOxygen;
    }

    public PlayerAttributes(Player player) : this(new Oxygen(100)) { }

    /// <summary>
    /// Increase the Oxygen of this Player about the given amount.
    /// If the Oxygen is already on maxOxygen, this call is ignored.
    /// </summary>
    /// <param name="amount">Amount of Oxygen to add</param>
    public void IncreaseOxygen(Oxygen amount)
    {
        if (CurrentOxygen + amount <= MaxOxygen) { CurrentOxygen += amount; }
        else { CurrentOxygen = MaxOxygen; }
        OxygenLevelChanged?.Invoke(CurrentOxygen);
    }

    /// <summary>
    /// Decrease the Oxygen of this Player about the given amount.
    /// If the Oxygen is 0 (or lower) IsAlive is set to false;
    /// </summary>
    /// <param name="amount">Amount of Oxygen to substract</param>
    public void DecreseOxygen(Oxygen amount)
    {
        if (CurrentOxygen > amount)
        {
            CurrentOxygen -= amount;
        }
        else
        {
            IsAlive = false;
            CurrentOxygen = new Oxygen(0);
        }
        OxygenLevelChanged?.Invoke(CurrentOxygen);
    }
}