using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class RealtimeController : MonoBehaviour
{
    float[] data = new float[10];

    Character player;
    Character enemy;
    float lastReceivedVersion;

    [DllImport("__Internal")]
    private static extern void TransmitRealtimeData(float[] array, int size);

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Character>();
        enemy = GameObject.FindWithTag("Enemy").GetComponent<Character>();

        // Длинна массива для обмена данными должна быть чётной
        Debug.Assert(data.Length % 2 == 0);

        try
        {
            TransmitRealtimeData(data, data.Length);
        }
        catch
        {
            enabled = false;

            Debug.LogWarning("RealtimeController disabled");
        }
    }

    // Update is called once per frame
    void Update()
    {
        var direction = Input.GetAxisRaw("Horizontal");
        var attack = Input.GetAxisRaw("Jump");
        var playerPosition = player.transform.position;

        if (direction != data[3] ||
            attack != data[4] ||
            playerPosition.x != data[1] ||
            playerPosition.y != data[2])
        {
            data[0] = Time.frameCount;
            data[1] = playerPosition.x;
            data[2] = playerPosition.y;
            data[3] = direction;
            data[4] = attack;
        }

        var receiveOffset = data.Length / 2;

        TransmitRealtimeData(data, receiveOffset);

        var receivedVersion = data[receiveOffset];

        if (receivedVersion > lastReceivedVersion)
        {
            lastReceivedVersion = receivedVersion;
            enemy.FixPosition(data[receiveOffset + 1], data[receiveOffset + 2]);
        }

        player.Control(direction, attack);
        enemy.Control(data[receiveOffset + 3], data[receiveOffset + 4]);
    }
}
