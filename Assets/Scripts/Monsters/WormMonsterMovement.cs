using UnityEngine;

namespace Monsters
{
    public class WormMonsterMovement : MonsterMovement
    {
        public float WaitAfterMoveS = 3f;
        public int steps = 4;
        
        private float _wait;
        private Vector3Int _cell;
        private bool _knowsPosition;
        private Monster _monster;

        private void Start()
        {
            _wait = WaitAfterMoveS;
            _knowsPosition = false;
            _monster = GetComponent<Monster>();
        }

        public override void MoveTowards(AttackableBuilding target)
        {
            if (!_knowsPosition)
            {
                _cell = _monster.Placeables.WorldToCell(transform.position);
                _knowsPosition = true;
            }

            if (_wait <= 0)
            {
                _wait = WaitAfterMoveS;
                _cell = target.NextCell(_cell, steps);

                var targetPos = _monster.Placeables.CellToWorld(_cell);
                
                // perturbate position a bit
                transform.position = new Vector3(targetPos.x + Random.Range(-0.5f, 0.5f),
                    targetPos.y + Random.Range(-0.5f, 0.0f),
                    transform.position.z);

                // make sure there is ground below the worm
            }
            else
            {
                _wait -= Time.fixedDeltaTime;
            }
        }
    }
}