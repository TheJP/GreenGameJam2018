using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Monsters
{
    [RequireComponent(typeof(Health))]
    public class AttackableBuilding : MonoBehaviour
    {
        // TODO: Calculate size dynamically
        private const int Size = 50;
        private const int Size2 = Size * 2;

        private const float DebugDist = Size2;

        public bool Recompute;
        public TileBase DebugTileSprite;
        public bool DrawDebugInfo;

        private Tilemap terrain;
        private Tilemap noColliders;
        private readonly int[] cost = new int[Size2 * Size2];

        private void Awake()
        {
            // TODO: Refactor (Don't rely on GameObject names. Also: Why is Pathfinding implemented in AttackableBuilding?)
            var grid = GameObject.Find("Grid");
            terrain = grid.GetComponentsInChildren<Tilemap>()
                .FirstOrDefault(t => t.gameObject.name == "CollisionTilemap");
            noColliders = grid.GetComponentsInChildren<Tilemap>()
                .FirstOrDefault(t => t.gameObject.name == "NoCollisionTilemap");

            if (terrain == null || noColliders == null)
            {
                Debug.LogError("Pathfinder relies on having a top-level object 'Grid' with the Tilemaps as children");
            }
        }

        private void Start()
        {
            ReloadPath();
        }

        public void ReloadPath()
        {
            CalcualteCostMatrix();
        }

        public MoveDirection PathFrom(Vector3 from)
        {
            var cell = terrain.WorldToCell(from);
            return PathFrom(cell);
        }

        public MoveDirection PathFrom(Vector3Int fromCell)
        {
            var position = fromCell;

            var left = Index(position + Vector3Int.left);
            var right = Index(position + Vector3Int.right);
            var up = Index(position + Vector3Int.up);
            var down = Index(position + Vector3Int.down);

            // order is important here
            // we prefere left/right over up and up over down

            var min = int.MaxValue;
            var nextDirection = MoveDirection.None;
            if (left >= 0 && cost[left] < min)
            {
                min = cost[left];
                nextDirection = MoveDirection.Left;
            }

            if (right >= 0 && cost[right] < min)
            {
                min = cost[right];
                nextDirection = MoveDirection.Right;
            }

            if (up >= 0 && cost[up] < min)
            {
                min = cost[up];
                nextDirection = MoveDirection.Up;
            }

            if (down >= 0 && cost[down] < min)
            {
                min = cost[down];
                nextDirection = MoveDirection.Down;
            }

            if (min == int.MaxValue)
            {
                Debug.Log("no path from " + fromCell + " to " + transform.position);
            }

            return nextDirection;
        }

        public Vector3Int NextCell(Vector3Int fromCell, int steps = 1)
        {

            var cell = fromCell;
            while (steps > 0)
            {
                steps--;
                var direction = PathFrom(cell);

                switch (direction)
                {
                    case MoveDirection.None:
                        steps = 0;
                        break;
                    case MoveDirection.Left:
                        cell += Vector3Int.left;
                        break;
                    case MoveDirection.Right:
                        cell += Vector3Int.right;
                        break;
                    case MoveDirection.Up:
                        cell += Vector3Int.up;
                        break;
                    case MoveDirection.Down:
                        cell += Vector3Int.down;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return cell;
        }

        private void Update()
        {
            if (Recompute)
            {
                Recompute = false;
                ReloadPath();
            }
        }

        private void Explore(Vector3Int position, int distance, Queue<Step> unexplored)
        {
            var index = Index(position);
            if (index < 0) { return; }

            if (terrain.GetTile(position) == null && cost[index] > distance)
            {
                unexplored.Enqueue(new Step(position, distance));
            }
        }

        private int Index(Vector3Int position)
        {
            var matX = position.x + Size;
            var matY = position.y + Size;

            if (matX < 0 || matX >= Size2 || matY < 0 || matY >= Size2)
            {
                return -1;
            }

            return matY * Size2 + matX;
        }

        private void CalcualteCostMatrix()
        {
            // TODO consider jumping ability when calulating path

            for (var i = 0; i < cost.Length; i++)
            {
                cost[i] = int.MaxValue;
            }

            if (Index(terrain.WorldToCell(transform.position)) < 0)
            {
                Debug.LogError($"Building {gameObject.name} at {transform.position} is outside the playing filed");
                return;
            }

            var unexplored = new Queue<Step>();
            unexplored.Enqueue(new Step(terrain.WorldToCell(transform.position), 0));

            while (unexplored.Count > 0)
            {
                var currentNode = unexplored.Dequeue();
                var position = currentNode.Position;

                var index = Index(position); // index is alwasy in bounds here

                if (cost[index] > currentNode.Cost)
                {
                    cost[index] = currentNode.Cost;
                    if (DrawDebugInfo)
                    {
                        noColliders.SetTile(position, DebugTileSprite);
                        noColliders.GetTile<Tile>(position).color =
                            Color.Lerp(Color.green, Color.red, currentNode.Cost / DebugDist);
                    }

                    int newCost = currentNode.Cost + 1;
                    Explore(position + Vector3Int.left, newCost, unexplored);
                    Explore(position + Vector3Int.right, newCost, unexplored);
                    Explore(position + Vector3Int.up, newCost, unexplored);
                    Explore(position + Vector3Int.down, newCost, unexplored);
                }
            }
        }
    }

    internal class Step
    {
        public Vector3Int Position { get; }
        public int Cost { get; }

        public Step(Vector3Int position, int cost)
        {
            this.Position = position;
            this.Cost = cost;
        }

        public override bool Equals(object obj)
        {
            var step = obj as Step;
            return step != null && Position.Equals(step.Position);
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }

    public enum MoveDirection
    {
        None,
        Left,
        Right,
        Up,
        Down
    }
}