using System;
using UnityEngine;

public class InventoryTile : IInventoryItem
{
    public Sprite Sprite { get { return Tile.Sprite; } set { throw new NotImplementedException(); } }
    public PrefabTile Tile { get; set; }

    public InventoryTile(PrefabTile tile)
    {
        this.Tile = tile;
    }
}

