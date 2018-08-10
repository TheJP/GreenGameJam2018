using UnityEngine;
using UnityEngine.Tilemaps;

namespace Monsters
{
    public class Monster : MonoBehaviour
    {
        public MonsterAttributes Attributes;

        public Transform Target;

        private Rigidbody2D _rigidbody2D;

        private void Start()
        {
            _rigidbody2D = FindObjectOfType<Rigidbody2D>();
        }

        private void Update()
        {
            
        }
    }
}