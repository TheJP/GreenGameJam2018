using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Pistole : MonoBehaviour, IPlayerWeapon
{
    public void Fire()
    {
        Debug.Log("PENG PENG");
    }
}