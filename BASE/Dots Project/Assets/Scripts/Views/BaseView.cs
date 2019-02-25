using UnityEngine;

public abstract class BaseView : MonoBehaviour
{
    public enum ViewKinds
    {
        MainMenu,
        Game
    }

    protected GameManager _gameManager;

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        if (_gameManager == null)
            _gameManager = GameManager.Instance;
    }
}
