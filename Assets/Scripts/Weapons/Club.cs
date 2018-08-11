using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Club : MonoBehaviour, IPlayerWeapon
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite[] clubSprites;

    public void Fire()
    {
        spriteRenderer.sprite = clubSprites[1];
    }
}