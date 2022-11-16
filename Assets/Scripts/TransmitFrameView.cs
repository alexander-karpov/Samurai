using UnityEngine;
using System;

public class TransmitFrameView
{
    public const int NumberOfFields = 5;

    const int accuracy = 100;
    const int updated = 0;
    const int x = 1;
    const int y = 2;
    const int inputX = 3;
    const int inputY = 4;

    readonly int[] data;
    readonly int offset;

    public TransmitFrameView(int[] data, int offset)
    {
        this.data = data;
        this.offset = offset;
    }

    public bool Updated
    {
        get => Convert.ToBoolean(data[updated + offset]);
        set => data[updated + offset] = Convert.ToInt32(value);
    }

    public Vector2 Position
    {
        get => new((float)data[x + offset] / accuracy, (float)data[y + offset] / accuracy);
        set
        {
            data[x + offset] = (int)(value.x * accuracy);
            data[y + offset] = (int)(value.y * accuracy);
        }
    }

    public Vector2 Input
    {
        get => new((float)data[inputX + offset] / accuracy, (float)data[inputY + offset] / accuracy);
        set
        {
            data[inputX + offset] = (int)(value.x * accuracy);
            data[inputY + offset] = (int)(value.y * accuracy);
        }
    }

    // public static bool operator ==(TransmitFrameView a, TransmitFrameView b)
    // {
    //     return a.Position == b.Position && a.Input == b.Input;
    // }

    // public static bool operator !=(TransmitFrameView a, TransmitFrameView b)
    // {
    //     return !(a == b);
    // }
}