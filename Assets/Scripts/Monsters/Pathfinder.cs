using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Monsters
{
    public class Pathfinder : MonoBehaviour
    {
        private const int Size = 30;
        private const int Size2 = Size * 2;

        private const float DebugDist = Size2;
        //private const int MaxJump = 3;

        public bool Recompute;
        public TileBase DebugTileSprite;
        public bool DrawDebugInfo;

        public Tilemap Terrain;
        public Tilemap NoColliders;


        Node[] _mat = new Node[Size2 * Size2];

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
            var position = Terrain.WorldToCell(from);

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
                Debug.Log("no path from " + from + " to " + transform.position);
            }

            return nextDirection;
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