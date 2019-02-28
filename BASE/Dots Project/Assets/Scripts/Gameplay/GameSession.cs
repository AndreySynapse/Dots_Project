using UnityEngine;

public class GameSession : MonoSingleton<GameSession>
{
    public ZBuffer CurrentZBuffer { get; set; }
    public PointRender CurrentPointRender { get { return _pointRender; } }
    public DotsSpaceRender CurrentSpaceRender { get { return _spaceRender; } }

    [SerializeField] private PointRender _pointRender;
    [SerializeField] private DotsSpaceRender _spaceRender;

    protected override void Init()
    {
        base.Init();

        this.CurrentZBuffer = new ZBuffer();
        this.CurrentZBuffer.Reset();
    }
}
