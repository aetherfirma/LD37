using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Minimap : MonoBehaviour
    {
        public Image DeadEnd, Straight, Corner, TJunction, Cross, Room;
        private ShipMaze _maze;
        private Canvas _self;
        private PlayerController _player;
        private Image[,] _minimap;
        private bool _created;
        private Image _currentCell;

        private void Start()
        {
            _maze = GameObject.Find("Controller").GetComponent<ShipMaze>();
            _self = GetComponent<Canvas>();
            _player = GameObject.Find("Player").GetComponent<PlayerController>();
        }

        public void GenerateMinimap()
        {
            var width = _maze.Maze.GetLength(0);
            var height = _maze.Maze.GetLength(1);

            _minimap = new Image[width, height];

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var cell = _maze.Maze[i, j];
                    Image icon;
                    if (cell == null) continue;
                    if (cell.Type() == CellType.Room) icon = Instantiate(Room);
                    else if (cell.Type() == CellType.Straight) icon = Instantiate(Straight);
                    else if (cell.Type() == CellType.Corner) icon = Instantiate(Corner);
                    else if (cell.Type() == CellType.TJunction) icon = Instantiate(TJunction);
                    else if (cell.Type() == CellType.Cross) icon = Instantiate(Cross);
                    else continue;

                    icon.transform.SetParent(_self.transform, false);
                    icon.rectTransform.anchoredPosition = new Vector2((i - width / 2) * 15, (j - height / 2) * 15);
                    icon.rectTransform.rotation = Quaternion.Euler(0, 0, -cell.Rotation().eulerAngles.y);
                    _minimap[i, j] = icon;
                    icon.enabled = false;
                }
            }
            _created = true;
        }

        private void Update()
        {
            if (!_created) return;
            var x = Mathf.RoundToInt(_player.transform.position.x / 10);
            var y = Mathf.RoundToInt(_player.transform.position.z / 10);
            var cell = _minimap[x, y];
            if (cell == null) return;
            if (_currentCell != null && cell != _currentCell) _currentCell.color = Color.white;
            cell.enabled = true;
            cell.color = Color.red;
            _currentCell = cell;
        }
    }
}