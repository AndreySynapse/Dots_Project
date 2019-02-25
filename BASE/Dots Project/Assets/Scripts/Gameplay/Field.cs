using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Vector2 _fieldSize;
    [SerializeField] private BoxCollider2D _triggerPrefab;
    [SerializeField] private float _normalizedCellSize;

    private Vector2 _cellSize;
    private Vector3 _startPosition;

    private void Awake()
    {
        float scaleFactor = 1f / _sprite.pixelsPerUnit; //_normalizedCellSize * (1f / _sprite.pixelsPerUnit);

        _cellSize = new Vector2((_sprite.rect.width / (_fieldSize.x - 1)) * scaleFactor, (_sprite.rect.height / (_fieldSize.y - 1)) * scaleFactor);
        print(_sprite.rect.width);
        print(_cellSize);

        _startPosition = this.transform.position + Vector3.left * _cellSize.x * (_fieldSize.x / 2f) + Vector3.up * _cellSize.y * (_fieldSize.y / 2f);
        _startPosition += Vector3.right * (_cellSize.x / 2f) + Vector3.down * (_cellSize.y / 2f);
    }

    private void Start()
    {
        for (int j = 0; j < _fieldSize.y; j++)
            for (int i = 0; i < _fieldSize.x; i++)
            {
                var cell = Instantiate(_triggerPrefab);
                cell.transform.position = _startPosition + Vector3.right * _cellSize.x * i + Vector3.down * _cellSize.y * j;
                cell.size = new Vector2(_cellSize.x * _normalizedCellSize, _cellSize.y * _normalizedCellSize);
                cell.gameObject.SetActive(true);

            }
    }

}
