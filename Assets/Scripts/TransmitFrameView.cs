using UnityEngine;
using System;

public struct TransmitFrameView
{
    public const int Size = 5;

    const int accuracy = 100;
    const int version = 0;
    const int x = 1;
    const int y = 2;
    const int inputX = 3;
    const int inputY = 4;

    readonly int[] data;
    readonly int offset;

    public TransmitFrameView(int[] data, int offset = 0)
    {
        this.data = data;
        this.offset = offset;
    }

    public int Version
    {
        get => data[version + offset];
        set => data[version + offset] = value;
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
}