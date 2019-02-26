using System;
using UnityEngine;

public enum CellStates
{
    Empty,
    Player1,
    Player2
}

public class CellTrigger : MonoBehaviour
{
    public event Action<CellTrigger> OnFillCell;

    public CellStates FillState { get; set; }
    public Vector2Int Index { get; set; }
    public BoxCollider2D CellCollider { get { return _collider; } }

    [SerializeField] private Transform _cachedTransform;
    [SerializeField] private BoxCollider2D _collider;

    private GameSession _gameSession;
    private bool _isFull;

    private void Awake()
    {
        if (_gameSession == null)
            _gameSession = GameSession.Instance;

        _isFull = false;
    }

    private void OnEnable()
    {
        _gameSession.CurrentZBuffer.OnChangeValue += OnChangeZBuffer;
    }

    private void OnDisable()
    {
        _gameSession.CurrentZBuffer.OnChangeValue -= OnChangeZBuffer;
    }

    private void OnMouseUpAsButton()
    {
        if (this.FillState == CellStates.Empty)
        {
            Transform point = _gameSession.CurrentPointRender.GetPoint();

            point.parent = _cachedTransform;
            point.localPosition = Vector3.zero;

            point.gameObject.SetActive(true);

            this.FillState = CellStates.Player1;
            OnFillCell.SafeInvoke(this);
        }
    }

    #region Events
    private void OnChangeZBuffer(float value)
    {
        Vector3 position = _cachedTransform.position;

        position.z = value;
        _cachedTransform.position = position;
    }
    #endregion
}
