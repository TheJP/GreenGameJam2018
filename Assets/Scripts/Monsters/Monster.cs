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
    public class Monster : MonoBehaviour
    {
        public MonsterAttributes Attributes;
        public Tilemap Placeables { get; set; }
        public Tilemap Terrain { get; set; }


        private Rigidbody2D _rigidbody2D;

        //private SpriteRenderer _spriteRenderer;
        //private float attacksPerSeconds = 1f;
        //private int stuckcount = 0;
        public AttackableBuilding Target;
        public MonsterMovement MonsterMovement;

        private AudioSource audioSource;
        private float originalPitch;
        private float pitchRange = 0.2f;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            //_spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

            audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.volume = 0.9f;
                audioSource.loop = true;
                audioSource.playOnAwake = false;
                originalPitch = audioSource.pitch;
                audioSource.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
                audioSource.Play();
            }

            if (Placeables == null)
            {
                Debug.LogError("Monster needs placeables tilemap");
            }
        }

        /*
        public void Awake()
        {
            Debug.Log("in awake");

            if (audioSource != null)
            {
                Debug.Log("have audio");
                audioSource.Play();
            }
        }
*/
        private bool InRange() => Target != null &&
                                  Vector3.Distance(Target.transform.position, transform.position) < Attributes.Range;

        private void Update()
        {
            if (Target == null)
            {
                Target = FindTarget();
                if (Target)
                {
                    Target = Target.gameObject.GetComponent<AttackableBuilding>();
                }
            }
        }

        private void FixedUpdate()
        {
            if (Target != null &&
                Vector3.Distance(Target.transform.position, transform.position) > Attributes.Range)
            {
                MonsterMovement.MoveTowards(Target);
            }

            if (InRange())
            {
                Target.GetComponent<Health>().TakeDamage(Attributes.Attackpower * Time.fixedDeltaTime);
            }
        }

        private AttackableBuilding FindTarget()
        {
            var attackable = Placeables.GetComponentsInChildren<AttackableBuilding>();
            if (attackable.Length == 0)
            {
                Debug.Log("using expensive FindObjectsOfType");
                attackable = FindObjectsOfType<AttackableBuilding>();
            }

            return attackable.Length == 0 ? null : attackable[Random.Range(0, attackable.Length)];
        }

        private MoveDirection _lastHorizontal = MoveDirection.Left;
    }
}