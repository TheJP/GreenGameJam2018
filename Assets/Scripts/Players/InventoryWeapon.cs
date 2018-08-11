using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class InventoryWeapon : IInventoryItem
{
    public Sprite Sprite { get; set; }
    public GameObject Weapon { get; private set; }

    public InventoryWeapon(GameObject weaponInstance, Sprite weaponImage)
    {
        this.Weapon = weaponInstance;
        this.Sprite = weaponImage;
    }
}

