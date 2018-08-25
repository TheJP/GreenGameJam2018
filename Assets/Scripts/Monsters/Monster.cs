using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Monsters
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(MonsterMovement))]
    public class Monster : MonoBehaviour
    {
        private const float PitchRange = 0.2f;

        public MonsterAttributes Attributes;
        public Tilemap Placeables { get; set; }
        public Tilemap Terrain { get; set; }

        public AttackableBuilding target;

        private MonsterMovement monsterMovement;
        private AudioSource audioSource;
        private float originalPitch;

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
            audioSource.pitch = Random.Range(originalPitch - PitchRange, originalPitch + PitchRange);
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

        public bool CanOnlyFall()
        {
            return monsterMovement.CanOnlyFall();
        }
    }
}