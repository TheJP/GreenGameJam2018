using System;
using System.Linq;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Rendering;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

namespace Monsters
{
    public class Monster : MonoBehaviour
    {
        public MonsterAttributes Attributes;

        public AttackableBuilding Target;

        private Rigidbody2D _rigidbody2D;
        //private SpriteRenderer _spriteRenderer;
        //private float attacksPerSeconds = 1f;
        //private int stuckcount = 0;
        private Pathfinder _pathfinder;
        
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
                audioSource.pitch = UnityEngine.Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);                
                audioSource.Play();
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
                    _pathfinder = Target.gameObject.GetComponent<Pathfinder>();
                }
            }


            if (InRange())
            {
                Attack();
            }
        }

        private void FixedUpdate()
        {
            if (Target != null &&
                Vector3.Distance(Target.transform.position, transform.position) > Attributes.Range - 1)
            {
                Move();
            }
        }

        private AttackableBuilding FindTarget()
        {
            var buildings = FindObjectsOfType<AttackableBuilding>();
            return buildings.FirstOrDefault();
        }

        private MoveDirection _lastHorizontal = MoveDirection.Left;

        private void Move()
        {
            var direction = _pathfinder.PathFrom(transform.position);
            if (direction == MoveDirection.Left || direction == MoveDirection.Down)
            {
                _lastHorizontal = direction;
            }

            if (_lastHorizontal == MoveDirection.Left)
            {
                transform.localScale = new Vector3(Math.Abs(transform.localScale.x) * -1, transform.localScale.y,
                    transform.localScale.z);
            }
            else if (Target.transform.position.x > transform.position.x + Attributes.Range)
            {
                transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y,
                    transform.localScale.z);
            }

            switch (direction)
            {
                case MoveDirection.Left:
                case MoveDirection.Right:
                    var forceDir = direction == MoveDirection.Left ? Vector3.left : Vector3.right;
                    _rigidbody2D.AddForce(forceDir * Attributes.MoveForce);
                    if (Mathf.Abs(_rigidbody2D.velocity.x) > Attributes.MaxSpeed)
                    {
                        _rigidbody2D.velocity = new Vector2(Mathf.Sign(_rigidbody2D.velocity.x) * Attributes.MaxSpeed,
                            _rigidbody2D.velocity.y);
                    }

                    break;
                case MoveDirection.Up:
                    _rigidbody2D.AddForce(new Vector2(0, Attributes.JumpForce));
                    break;
                case MoveDirection.Down:
                case MoveDirection.None:
                    // change animation to idle
                    _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Attack()
        {
            if ((Target.transform.position - transform.position).magnitude < Attributes.Range)
            {
                Target.TakeDamage(Attributes.Attackpower * Time.deltaTime);
            }
        }
    }
}