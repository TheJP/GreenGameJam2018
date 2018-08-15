using System;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Rendering;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;
using Random = UnityEngine.Random;

namespace Monsters
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(MonsterMovement))]
    public class Monster : MonoBehaviour
    {
        public MonsterAttributes Attributes;
        public Tilemap Placeables { get; set; }
        public Tilemap Terrain { get; set; }

        public AttackableBuilding target;

        private MonsterMovement monsterMovement;
        private AudioSource audioSource;
        private float originalPitch;
        private float pitchRange = 0.2f;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            monsterMovement = GetComponent<MonsterMovement>();
        }

        private void Start()
        {
            audioSource.volume = 0.9f;
            audioSource.loop = true;
            audioSource.playOnAwake = false;
            originalPitch = audioSource.pitch;
            audioSource.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
            audioSource.Play();

            Debug.Assert(Placeables != null, "Monster needs placeables tilemap");
        }

        private bool InRange() =>
            target != null && Vector3.Distance(target.transform.position, transform.position) < Attributes.Range;

        private void Update()
        {
            if (target == null)
            {
                target = FindTarget();
            }
        }

        private void FixedUpdate()
        {
            if (target != null &&
                Vector3.Distance(target.transform.position, transform.position) > Attributes.Range)
            {
                monsterMovement.MoveTowards(target);
            }

            if (InRange())
            {
                target.GetComponent<Health>().TakeDamage(Attributes.Attackpower * Time.fixedDeltaTime);
            }
        }

        private AttackableBuilding FindTarget()
        {
            var attackable = Placeables.GetComponentsInChildren<AttackableBuilding>();
            return attackable.Length == 0 ? null : attackable[Random.Range(0, attackable.Length)];
        }
    }
}