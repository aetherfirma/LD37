using UnityEngine;

namespace Assets.Scripts
{
    internal class MazeCell
    {
        public GameObject Straight, Corner, TJunction, Cross, DeadEnd;

        // Up +Z, Right +X
        public MazeCell Up, Down, Left, Right;

        public int X, Y;
        private readonly GameObject[] _gameObjectLookup;

        public MazeCell(int x, int y, GameObject straight, GameObject corner, GameObject tJunction, GameObject cross, GameObject deadEnd)
        {
            X = x;
            Y = y;

            Straight = straight;
            Corner = corner;
            TJunction = tJunction;
            Cross = cross;
            DeadEnd = deadEnd;

            _gameObjectLookup = new[]
            {
                null,
                DeadEnd,
                null,
                TJunction,
                Cross
            };
        }

        public void SetRelationship(MazeCell cell)
        {
            Debug.Log(X + "," + Y + " -> " + cell.X + "," + cell.Y);
            if (cell.X == X + 1)
            {
                Right = cell;
                cell.Left = this;
            } else if (cell.X == X - 1)
            {
                Left = cell;
                cell.Right = this;
            } else if (cell.Y == Y + 1)
            {
                Up = cell;
                cell.Down = this;
            } else if (cell.Y == Y - 1)
            {
                Down = cell;
                cell.Up = this;
            }
        }

        public int Connections()
        {
            var connections = 0;
            if (Up != null) connections += 1;
            if (Down != null) connections += 1;
            if (Left != null) connections += 1;
            if (Right != null) connections += 1;
            return connections;
        }

        public GameObject GameObject()
        {
            if (Connections() == 2)
            {
                return (Up != null || Down != null) && (Left != null || Right != null) ? Corner : Straight;
            }
            return _gameObjectLookup[Connections()];
        }

        public Quaternion Rotation()
        {
            if (GameObject() == DeadEnd)
            {
                if (Down != null) return Quaternion.Euler(0, 0, 0);
                if (Left != null) return Quaternion.Euler(0, 90, 0);
                if (Up != null) return Quaternion.Euler(0, 180, 0);
                if (Right != null) return Quaternion.Euler(0, 270, 0);
            }
            if (GameObject() == Straight)
            {
                if (Down != null) return Quaternion.Euler(0, 0, 0);
                if (Left != null) return Quaternion.Euler(0, 90, 0);
            }
            if (GameObject() == Corner)
            {
                if (Down != null && Right != null) return Quaternion.Euler(0, 0, 0);
                if (Down != null && Left != null) return Quaternion.Euler(0, 90, 0);
                if (Up != null && Left != null) return Quaternion.Euler(0, 180, 0);
                if (Up != null && Right != null) return Quaternion.Euler(0, 270, 0);
            }
            if (GameObject() == TJunction)
            {
                if (Down != null && Right != null && Left != null) return Quaternion.Euler(0, 0, 0);
                if (Down != null && Left != null && Up != null) return Quaternion.Euler(0, 90, 0);
                if (Up != null && Left != null && Right != null) return Quaternion.Euler(0, 180, 0);
                if (Up != null && Right != null && Down != null) return Quaternion.Euler(0, 270, 0);
            }
            return Quaternion.identity;
        }

        public Vector3 Position(int floor = 0)
        {
            return new Vector3(X * 10, floor * 10, Y * 10);
        }
    }
}