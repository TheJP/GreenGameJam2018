using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Club : MonoBehaviour, IPlayerWeapon
{
    [SerializeField]
    private Animator animator;

    public void Fire()
    {
        animator.SetTrigger("beat");
    }
}