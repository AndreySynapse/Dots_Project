using System;

public class ZBuffer
{
    public event Action<float> OnChangeValue;

    public float Value { get; private set; }

    public void Reset()
    {
        this.Value = -1f;
    }

    public void ChangeValue()
    {
        this.Value--;
        OnChangeValue.SafeInvoke(this.Value);
    }
}
