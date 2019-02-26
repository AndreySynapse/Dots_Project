using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Vector2Int _fieldSize;
    [SerializeField] private Transform _container;

    [Header("Cell configuration")]
    [SerializeField] private CellTrigger _cellTriggerPrefab;
    [SerializeField] private float _normalizedCellSize;

    private GameSession _gameSession;
    private Vector2 _cellSize;
    private Vector3 _startPosition;

    private CellTrigger[,] _field;
    
    private void Awake()
    {
        if (_gameSession == null)
            _gameSession = GameSession.Instance;

        float scaleFactor = 1f / _sprite.pixelsPerUnit;
        _cellSize = new Vector2((_sprite.rect.width / (_fieldSize.x - 1)) * scaleFactor, (_sprite.rect.height / (_fieldSize.y - 1)) * scaleFactor);

        float zPosition = _gameSession.CurrentZBuffer.Value;
        _startPosition = this.transform.position + Vector3.left * _cellSize.x * (_fieldSize.x / 2f) + Vector3.up * _cellSize.y * (_fieldSize.y / 2f) + Vector3.forward * zPosition;
        _startPosition += Vector3.right * (_cellSize.x / 2f) + Vector3.down * (_cellSize.y / 2f);
    }

    private void Start()
    {
        _field = new CellTrigger[_fieldSize.x, _fieldSize.y];

        for (int j = 0; j < _fieldSize.y; j++)
            for (int i = 0; i < _fieldSize.x; i++)
            {
                var cell = Instantiate(_cellTriggerPrefab);

                cell.transform.position = _startPosition + Vector3.right * _cellSize.x * i + Vector3.down * _cellSize.y * j;
                cell.CellCollider.size = new Vector2(_cellSize.x * _normalizedCellSize, _cellSize.y * _normalizedCellSize);
                cell.FillState = CellStates.Empty;
                cell.Index = new Vector2Int(i, j);
                cell.transform.parent = _container;

                cell.gameObject.SetActive(true);

                _field[i, j] = cell;
                cell.OnFillCell += OnFillCell;
            }
    }

    private void OnFillCell(CellTrigger cell)
    {
        Vector2 start = cell.Index;

        List<Vector2> usedCells = new List<Vector2>();
        usedCells.Add(cell.Index);

        
                                
        var res = GetNearPoints(cell);

        while (true)
        {

        }
    }

    private List<CellTrigger> GetNearPoints(CellTrigger cell)
    {
        int minX = cell.Index.x > 0 ? cell.Index.x - 1 : 0;
        int maxX = cell.Index.x < _fieldSize.x - 1 ? cell.Index.x + 1 : _fieldSize.x - 1;
        int minY = cell.Index.y > 0 ? cell.Index.y - 1 : 0;
        int maxY = cell.Index.y < _fieldSize.y - 1 ? cell.Index.y + 1 : _fieldSize.y - 1;
        
        List<CellTrigger> results = new List<CellTrigger>();

        for (int j = minY; j <= maxY; j++)
            for (int i = minX; i <= maxX; i++)
                if (_field[i, j].FillState == cell.FillState && !(i == cell.Index.x && j == cell.Index.y))
                    results.Add(_field[i, j]);

        return results;
    }
}
