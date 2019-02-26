using UnityEngine;

public class GameSession : MonoSingleton<GameSession>
{
    public ZBuffer CurrentZBuffer { get; set; }
    public PointRender CurrentPointRender { get { return _pointRender; } }

    [SerializeField] private PointRender _pointRender;

    protected override void Init()
    {
        base.Init();

        this.CurrentZBuffer = new ZBuffer();
        this.CurrentZBuffer.Reset();
    }
}
