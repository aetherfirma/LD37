using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class ShipMaze : MonoBehaviour
    {
        public GameObject Straight, Corner, TJunction, Cross, DeadEnd, Door;
        public Vector2 MazeSize = new Vector2(10, 10);

        private void Start()
        {
            GenerateMaze();
        }

        private void GenerateMaze()
        {
            var maze = new MazeCell[(int)MazeSize.x, (int)MazeSize.y];
            var unvisited = new HashSet<MazeCell>();
            var doors = new HashSet<Vector3>();
            for (var x = 0; x < MazeSize.x; x++)
            {
                for (var y = 0; y < MazeSize.y; y++)
                {
                    var mazeCell = new MazeCell(x, y, Straight, Corner, TJunction, Cross, DeadEnd);
                    unvisited.Add(mazeCell);
                    maze[x, y] = mazeCell;
                }
            }

            var stack = new Stack<MazeCell>();
            var cell = RandomWithoutReplacementFromHashSet(unvisited);
            unvisited.Remove(cell);

            while (unvisited.Count > 0)
            {
                var candidates = Neighbours(maze, cell);
                candidates.IntersectWith(unvisited);
                while (candidates.Count > 0)
                {
                    var newCell = RandomWithoutReplacementFromHashSet(candidates);
                    cell.SetRelationship(newCell);
                    stack.Push(cell);
                    cell = newCell;
                    unvisited.Remove(cell);

                    candidates = Neighbours(maze, cell);
                    candidates.IntersectWith(unvisited);
                }
                if (stack.Count == 0) break;
                cell = stack.Pop();
            }

            for (var y = 0; y < MazeSize.y; y++)
            {
                for (var x = 0; x < MazeSize.x; x++)
                {
                    var mazeCell = maze[x, y];
                    Debug.Log(mazeCell);
                    Instantiate(mazeCell.GameObject(), mazeCell.Position(), mazeCell.Rotation(), transform);
                    PlaceDoors(mazeCell, doors);
                }
            }
        }

        private void PlaceDoors(MazeCell cell, HashSet<Vector3> doors)
        {
            Vector3 upDoor = cell.Position() + new Vector3(0, 0 , 5), downDoor = cell.Position() + new Vector3(0, 0, -5),
                rightDoor = cell.Position() + new Vector3(5, 0, 0), leftDoor = cell.Position() + new Vector3(-5, 0, 0);
            Quaternion leftRight = Quaternion.Euler(0, 90, 0);
            if (cell.Up != null && Random.value > 0.75 && !doors.Contains(upDoor))
                Instantiate(Door, upDoor, Quaternion.identity, transform);
            if (cell.Down != null && Random.value > 0.75 && !doors.Contains(downDoor))
                Instantiate(Door, downDoor, Quaternion.identity, transform);
            if (cell.Left != null && Random.value > 0.75 && !doors.Contains(leftDoor))
                Instantiate(Door, leftDoor, leftRight, transform);
            if (cell.Right != null && Random.value > 0.75 && !doors.Contains(rightDoor))
                Instantiate(Door, rightDoor, leftRight, transform);
        }

        private static void DebugLines(MazeCell cell)
        {
            if (cell.Up != null)
            {
                Debug.DrawLine(cell.Position(), cell.Position() + new Vector3(0, 1, 10), Color.blue, 1000);
            }
            if (cell.Down != null)
            {
                Debug.DrawLine(cell.Position(), cell.Position() + new Vector3(0, 1, -10), Color.green, 1000);
            }
            if (cell.Left != null)
            {
                Debug.DrawLine(cell.Position(), cell.Position() + new Vector3(-10, 1, 0), Color.red, 1000);
            }
            if (cell.Right != null)
            {
                Debug.DrawLine(cell.Position(), cell.Position() + new Vector3(10, 1, 0), Color.yellow, 1000);
            }
        }

        private static MazeCell RandomWithoutReplacementFromHashSet(HashSet<MazeCell> hash)
        {
            var array = hash.ToArray();
            var cell = array[Mathf.RoundToInt(Random.value * (array.Length - 1))];
            hash.Remove(cell);
            return cell;
        }

        private HashSet<MazeCell> Neighbours(MazeCell[,] maze, MazeCell location)
        {
            var neighbours = new HashSet<MazeCell>();
            int nx, ny;

            nx = location.X + 1;
            ny = location.Y + 0;
            if (nx >= 0  && nx < MazeSize.x && ny >= 0 && ny < MazeSize.y) neighbours.Add(maze[nx, ny]);
            nx = location.X + 0;
            ny = location.Y + 1;
            if (nx >= 0  && nx < MazeSize.x && ny >= 0 && ny < MazeSize.y) neighbours.Add(maze[nx, ny]);
            nx = location.X - 1;
            ny = location.Y + 0;
            if (nx >= 0  && nx < MazeSize.x && ny >= 0 && ny < MazeSize.y) neighbours.Add(maze[nx, ny]);
            nx = location.X + 0;
            ny = location.Y - 1;
            if (nx >= 0  && nx < MazeSize.x && ny >= 0 && ny < MazeSize.y) neighbours.Add(maze[nx, ny]);

            return neighbours;
        }
    }
}