using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class InventoryWeapon : IInventoryItem
{
    public Sprite Sprite { get; set; }

    public InventoryWeapon(Sprite weaponImage)
    {
        this.Sprite = weaponImage;
    }
}

