using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class OxygenTowerParticleSystem : MonoBehaviour
{
    [Tooltip("Particel system that gets activated and deactivated")]
    public GameObject particles;

    private Animator animator;

    private void Start() => animator = GetComponent<Animator>();

    private void Update() => particles.SetActive(animator.GetBool(OxygenTower.AnimatorOnFlag));
}
