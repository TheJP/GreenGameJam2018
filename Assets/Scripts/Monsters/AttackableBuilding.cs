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
        private const int Size = 30;
        private const int Size2 = Size * 2;

        private const float DebugDist = Size2;
        //private const int MaxJump = 3;

        public bool Recompute;
        public TileBase DebugTileSprite;
        public bool DrawDebugInfo;

        private Tilemap Terrain;
        private Tilemap NoColliders;
        Node[] _mat = new Node[Size2 * Size2];

        private void Awake()
        {
            var grid = GameObject.Find("Grid");
            Terrain = grid.GetComponentsInChildren<Tilemap>()
                .FirstOrDefault(t => t.gameObject.name == "CollisionTilemap");
            NoColliders = grid.GetComponentsInChildren<Tilemap>()
                .FirstOrDefault(t => t.gameObject.name == "NoCollisionTilemap");

            if (Terrain == null || NoColliders == null)
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
            _mat = CalcualteCostMatrix();
        }

        public MoveDirection PathFrom(Vector3 from)
        {
            var cell = Terrain.WorldToCell(from);
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
            if (left >= 0 && _mat[left].Distance < min)
            {
                min = _mat[left].Distance;
                nextDirection = MoveDirection.Left;
            }

            if (right >= 0 && _mat[right].Distance < min)
            {
                min = _mat[right].Distance;
                nextDirection = MoveDirection.Right;
            }

            if (up >= 0 && _mat[up].Distance < min)
            {
                min = _mat[up].Distance;
                nextDirection = MoveDirection.Up;
            }

            if (down >= 0 && _mat[down].Distance < min)
            {
                min = _mat[down].Distance;
                nextDirection = MoveDirection.Down;
            }

            if (min == int.MaxValue)
            {
                Debug.Log("no path from " + fromCell + " to " + transform.position);
            }

            return nextDirection;
        }

        public Vector3Int CellPosition(Vector3 position)
        {
            return Terrain.WorldToCell(position);
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

        private void Explore(Vector3Int position, Node[] cost, int distance, Queue<Step> unexplored)
        {
            var matX = position.x + Size;
            var matY = position.y + Size;
            if (matX < 0 || matX >= Size2 || matY < 0 || matY >= Size2)
            {
                return;
            }

            var index = matY * Size2 + matX;
            if (Terrain.GetTile(position) == null && cost[index].Distance > distance + 1)
            {
                unexplored.Enqueue(new Step(position, distance + 1));
            }
        }

        private int Index(Vector3Int position)
        {
            var matX = position.x + Size;
            var matY = position.y + Size;
            var index = matY * Size2 + matX;

            if (matX < 0 || matX >= Size2 || matY < 0 || matY >= Size2)
            {
                return -1;
            }

            return index;
        }

        private Node[] CalcualteCostMatrix()
        {
            // TODO consider jumping ability when calulating path

            Node[] cost = new Node[Size2 * Size2];
            for (var i = 0; i < cost.Length; i++)
            {
                cost[i] = new Node(int.MaxValue);
            }

            if (Index(Terrain.WorldToCell(transform.position)) < 0)
            {
                Debug.LogError($"Building {gameObject.name} at {transform.position} is outside the playing filed");
                return cost;
            }


            var unexplored = new Queue<Step>(Size2);
            unexplored.Enqueue(new Step(Terrain.WorldToCell(transform.position), 0));

            while (unexplored.Count > 0)
            {
                var currentNode = unexplored.Dequeue();
                var position = currentNode.Position;

                var index = Index(position); // index is alwasy in bounds here

                if (cost[index].Distance > currentNode.Cost)
                {
                    cost[index].Distance = currentNode.Cost;
                    if (DrawDebugInfo)
                    {
                        NoColliders.SetTile(position, DebugTileSprite);
                        NoColliders.GetTile<Tile>(position).color =
                            Color.Lerp(Color.green, Color.red, currentNode.Cost / DebugDist);
                    }

                    Explore(position + Vector3Int.left, cost, currentNode.Cost, unexplored);
                    Explore(position + Vector3Int.right, cost, currentNode.Cost, unexplored);
                    Explore(position + Vector3Int.up, cost, currentNode.Cost, unexplored);
                    Explore(position + Vector3Int.down, cost, currentNode.Cost, unexplored);
                }
            }

            return cost;
        }
    }

    internal class Node
    {
        public int Distance { get; set; }

        public Node(int distance, int jump = 0)
        {
            Distance = distance;
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