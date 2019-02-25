using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private enum PivotMode
    {
        Center,
        Left,
        Right
    }
    
    [SerializeField] private GameObject _targetCell;
    [SerializeField] private PivotMode _pivot;
    [SerializeField] private float _cellSize;
    [SerializeField] private Vector2 _mapSize;
    [SerializeField] private Vector2 _positionOffset;
    

    void Start()
    {
        GenerateMap();
    }

    void Update()
    {
        
    }

    private void GenerateMap()
    {
        Vector2 position = Vector2.zero;

        switch (_pivot)
        {
            case PivotMode.Center:
                position += Vector2.left * _cellSize * (_mapSize.x / 2);
                break;

            case PivotMode.Left:
                break;

            case PivotMode.Right:
                position += Vector2.left * _cellSize * _mapSize.x;
                break;
        }
        
        for (int i = 0; i < _mapSize.x; i++)
            //for (int j = 0; j < _mapSize.y; j++)
            {
                var cell = Instantiate(_targetCell);

                cell.transform.position = position + Vector2.right * i * _cellSize; 
                
                
            }
    }
}
