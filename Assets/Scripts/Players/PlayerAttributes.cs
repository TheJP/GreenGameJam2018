public class PlayerAttributes
{
    public int MaxOxygen { get; set; }
    public bool IsAlive { get; set; }

#pragma warning disable 0649
    private Player player;
#pragma warning restore 0649

    private int currentOxygen;

    //List of Phobies
    //Equipped Weapon/Tower
    //Weapon Inventory
    //Tower Inventory

    public PlayerAttributes(Player player, int maxOxygen)
    {
        this.player = player;
        MaxOxygen = maxOxygen;
        this.currentOxygen = maxOxygen;
    }

    public PlayerAttributes(Player player) : this(player, 100)
    {
    }

    /// <summary>
    /// Increase the Oxygen of this Player about the given amount.
    /// If the Oxygen is already on maxOxygen, this call is ignored.
    /// </summary>
    /// <param name="amount">Amount of Oxygen to add</param>
    public void IncreaseOxygen(int amount)
    {
        if (this.currentOxygen < this.MaxOxygen)
        {
            currentOxygen++;
        }
    }

    /// <summary>
    /// Decrease the Oxygen of this Player about the given amount.
    /// If the Oxygen is 0 (or lower) IsAlive is set to false;
    /// </summary>
    /// <param name="amount">Amount of Oxygen to substract</param>
    public void DecreseOxygen(int amount)
    {
        if (this.currentOxygen > 0)
        {
            this.currentOxygen--;
        }
        else
        {
            IsAlive = false;
            player.StopRefillOxgen();
            player.StopConsumeOxygen();
        }
    }
}