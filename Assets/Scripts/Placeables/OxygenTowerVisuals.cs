using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(OxygenTower))]
public class OxygenTowerVisuals : MonoBehaviour
{
    [Tooltip("Particel system that gets activated and deactivated")]
    public GameObject particles;

    [Tooltip("Game object that visualises the area of effect of the oxygen tower")]
    public GameObject oxygenArea;

    private Animator animator;
    private OxygenTower oxygenTower;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        oxygenTower = GetComponent<OxygenTower>();
    }

    private void Update()
    {
        bool on = animator.GetBool(OxygenTower.AnimatorOnFlag);
        particles.SetActive(on);
        oxygenArea.SetActive(on);

        if (on) { oxygenArea.transform.localScale = new Vector3(oxygenTower.aoeRadius * 2f, oxygenTower.aoeRadius * 2f, 1f); }
    }
}
