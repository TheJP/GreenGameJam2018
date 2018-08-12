using System;
using UnityEngine;

namespace Monsters
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ArachnidMonsterMovement : MonsterMovement
    {
        private MoveDirection _lastHorizontal = MoveDirection.Left;
        private Rigidbody2D _rigidbody2D;
        private MonsterAttributes _attributes;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _attributes = GetComponent<MonsterAttributes>();
        }


        public override void MoveTowards(AttackableBuilding target)
        {
            var direction = target.PathFrom(transform.position);
            if (direction == MoveDirection.Left || direction == MoveDirection.Down)
            {
                _lastHorizontal = direction;
            }

            if (_lastHorizontal == MoveDirection.Left)
            {
                transform.localScale = new Vector3(Math.Abs(transform.localScale.x) * -1, transform.localScale.y,
                    transform.localScale.z);
            }
            else if (target.transform.position.x > transform.position.x + _attributes.Range)
            {
                transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y,
                    transform.localScale.z);
            }

            switch (direction)
            {
                case MoveDirection.Left:
                case MoveDirection.Right:
                    var forceDir = direction == MoveDirection.Left ? Vector3.left : Vector3.right;
                    _rigidbody2D.AddForce(forceDir * _attributes.MoveForce);
                    if (Mathf.Abs(_rigidbody2D.velocity.x) > _attributes.MaxSpeed)
                    {
                        _rigidbody2D.velocity = new Vector2(Mathf.Sign(_rigidbody2D.velocity.x) * _attributes.MaxSpeed,
                            _rigidbody2D.velocity.y);
                    }

                    break;
                case MoveDirection.Up:
                    _rigidbody2D.AddForce(new Vector2(0, _attributes.JumpForce));
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
    }
}