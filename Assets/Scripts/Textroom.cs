using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class Textroom : MonoBehaviour
{
    readonly int[] transmissionBuffer = new int[16];

    //
    // Livecycle
    //

    void Start()
    {
        try
        {
            Init();
            Join();
        }
        catch
        {
            enabled = false;
            Debug.LogWarning("Textroom disabled");
        }
    }

    void FixedUpdate()
    {
        ReceiveAll();
    }

    //
    // Events
    //

    public event EventHandler<string> OnEvent;
    public event EventHandler<(int version, Vector2 input, Vector2 position)> OnInput;

    //
    // Api
    //


    public void SendInput(int version, Vector2 input, Vector2 position)
    {
        if (!enabled)
        {
            return;
        }

        var encoder = new TextroomMessageEncoder(transmissionBuffer);

        encoder.Write(TextroomMessageType.Input);
        encoder.Write(version);
        encoder.Write(input);
        encoder.Write(position);

        Send(transmissionBuffer, encoder.Size);

#if DEVELOPMENT_BUILD
        Debug.Log($"Send Input {string.Join(", ", transmissionBuffer)}");
#endif
    }

    void ReceiveAll()
    {
        while (true)
        {
            /*     
            Вернувшееся значение отвечает на два вопроса:
                1. Было бы вообще что-то считано (если не 0)
                2. Нужно ли ещё что-то считывать (если 2 или больше)
            */
            var countBeforeRead = Receive(transmissionBuffer, transmissionBuffer.Length);
            var decoder = new TextroomMessageDecoder(transmissionBuffer);

            if (countBeforeRead != 0)
            {
#if DEVELOPMENT_BUILD
                Debug.Log($"Receive message {string.Join(", ", transmissionBuffer)}");
#endif
                switch (decoder.ReadType())
                {
                    case TextroomMessageType.Input:
                        OnInput?.Invoke(this, (decoder.ReadInt(), decoder.ReadVector2(), decoder.ReadVector2()));
                        break;
                }
            }

            if (countBeforeRead < 2)
            {
                return;
            }
        }
    }

    //
    // JavaScript interop
    //

    [DllImport("__Internal")]
    private static extern void Init();

    [DllImport("__Internal")]
    private static extern void Join();

    [DllImport("__Internal")]
    private static extern void Send(int[] data, int size);

    /// <returns>Количество сообщений ДО считывания</returns>
    [DllImport("__Internal")]
    private static extern int Receive(int[] data, int size);

    private void RaiseEvent(string payload)
    {
#if DEVELOPMENT_BUILD
        Debug.Log($"Event {payload}");
#endif

        OnEvent?.Invoke(this, payload);
    }
}


enum TextroomMessageType : int
{
    Input = 1
}

struct TextroomMessageEncoder
{
    public const int FloatToIntAccuracy = 100;

    readonly int[] _buffer;
    int _offset;

    public TextroomMessageEncoder(int[] buffer, int offset = 0)
    {
        _buffer = buffer;
        _offset = offset;
    }

    public int Size => _offset;

    public void Write(TextroomMessageType type)
    {
        Write((int)type);
    }

    public void Write(Vector2 value)
    {
        Write(value.x);
        Write(value.y);
    }

    public void Write(int value)
    {
        _buffer[_offset++] = value;
    }

    public void Write(float value)
    {
        _buffer[_offset++] = (int)(value * FloatToIntAccuracy);

    }
}

struct TextroomMessageDecoder
{
    public const int FloatToIntAccuracy = TextroomMessageEncoder.FloatToIntAccuracy;

    readonly int[] _buffer;
    int _offset;

    public TextroomMessageDecoder(int[] buffer, int offset = 0)
    {
        _buffer = buffer;
        _offset = offset;
    }

    public TextroomMessageType ReadType()
    {
        return (TextroomMessageType)ReadInt();
    }

    public Vector2 ReadVector2()
    {
        return new(ReadFloat(), ReadFloat());
    }

    public int ReadInt()
    {
        return _buffer[_offset++];
    }

    public float ReadFloat()
    {
        return (float)_buffer[_offset++] / FloatToIntAccuracy;
    }
}