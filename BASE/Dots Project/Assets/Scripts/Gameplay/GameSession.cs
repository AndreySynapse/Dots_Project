using UnityEngine;

public class GameSession : MonoSingleton<GameSession>
{
    public ZBuffer CurrentZBuffer { get; set; }

    protected override void Init()
    {
        base.Init();

        this.CurrentZBuffer = new ZBuffer();
        this.CurrentZBuffer.Reset();
    }
}
