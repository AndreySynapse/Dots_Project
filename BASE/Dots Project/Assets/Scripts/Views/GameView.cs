using UnityEngine;

public class GameView : BaseView
{
    private GameSession _gameSession;

    protected override void Init()
    {
        base.Init();

        if (_gameSession == null)
            _gameSession = GameSession.Instance;
    }
}
